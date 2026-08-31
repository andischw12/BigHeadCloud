mergeInto(LibraryManager.library, {
    $BigHeadStorage: {
        endpoint: "/asp/BigHeadGameData.asp",
        gameKey: 2,
        prefix: "meirkids.bighead2.",
        queues: {},

        pointerToString: function (pointer) {
            if (typeof UTF8ToString === "function") return UTF8ToString(pointer);
            return Pointer_stringify(pointer);
        },

        returnString: function (value) {
            if (!value) return 0;
            var bufferSize = lengthBytesUTF8(value) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(value, buffer, bufferSize);
            return buffer;
        },

        cookieOid: function () {
            var match = document.cookie.match(/(?:^|;\s*)UserSettings=([^;]*)/i);
            if (!match) return "browser";
            var value = match[1].replace(/\+/g, " ");
            for (var i = 0; i < 2; i++) {
                try { value = decodeURIComponent(value); } catch (ignore) { break; }
            }
            var oidMatch = value.match(/"OID"\s*:\s*"([A-Za-z0-9_-]{1,200})"/i);
            return oidMatch ? oidMatch[1] : "browser";
        },

        playerKey: function (player) {
            return this.prefix + this.cookieOid() + ".player." + player;
        },

        parseData: function (rawData) {
            try {
                var data = typeof rawData === "string" ? JSON.parse(rawData) : rawData;
                return data && Array.isArray(data.UserGeneralInfoArr) ? data : null;
            } catch (ignore) {
                return null;
            }
        },

        pointsFrom: function (data, fallback) {
            var points = data && data.UserGeneralInfoArr ? parseInt(data.UserGeneralInfoArr[4], 10) : NaN;
            return isFinite(points) ? points : (parseInt(fallback, 10) || 0);
        },

        normalizedName: function (data) {
            return String(data && data.UserName || "").toLowerCase()
                .replace(/[?"'\t\r\n]/g, "").replace(/^\s+|\s+$/g, "");
        },

        backupConflict: function (player, envelope) {
            try {
                localStorage.setItem(this.playerKey(player) + ".conflict." +
                    (envelope.savedAt || Date.now()), JSON.stringify(envelope));
            } catch (ignore) { }
        },

        readEnvelope: function (player) {
            var key = this.playerKey(player);
            var raw = localStorage.getItem(key);
            var cameFromLegacyKey = false;
            if (!raw) raw = localStorage.getItem("BigHead_2" + player);
            if (!raw) raw = localStorage.getItem("BigHead" + player + "_2");
            if (raw && !localStorage.getItem(key)) cameFromLegacyKey = true;
            if (!raw) return null;

            try {
                var parsed = JSON.parse(raw);
                var data;
                if (parsed && typeof parsed.data === "string") {
                    data = this.parseData(parsed.data);
                    if (!data) {
                        this.backupConflict(player, parsed);
                        console.warn("BigHead 2: rejected local data with a different game layout");
                        return null;
                    }
                    parsed.points = this.pointsFrom(data, parsed.points);
                    parsed.savedAt = parseInt(parsed.savedAt, 10) || Date.now();
                    parsed.dirty = parsed.dirty === true;
                    parsed.legacy = cameFromLegacyKey || parsed.legacy === true;
                    localStorage.setItem(key, JSON.stringify(parsed));
                    return parsed;
                }

                data = this.parseData(parsed);
                if (!data) {
                    console.warn("BigHead 2: ignored a BigHead 1 local-storage record");
                    return null;
                }

                var migrated = {
                    data: JSON.stringify(data),
                    points: this.pointsFrom(data, 0),
                    shabbatPoints: 0,
                    hanukkaPoints: 0,
                    purimPoints: 0,
                    savedAt: Date.now(),
                    dirty: !cameFromLegacyKey,
                    legacy: cameFromLegacyKey
                };
                localStorage.setItem(key, JSON.stringify(migrated));
                return migrated;
            } catch (error) {
                console.warn("BigHead 2: invalid local data for player " + player, error);
                return null;
            }
        },

        writeEnvelope: function (player, envelope) {
            try {
                localStorage.setItem(this.playerKey(player), JSON.stringify(envelope));
                return true;
            } catch (error) {
                console.warn("BigHead 2: local save failed", error);
                return false;
            }
        },

        requestSync: function (action, query) {
            try {
                var xhr = new XMLHttpRequest();
                var url = this.endpoint + "?action=" + encodeURIComponent(action) +
                    "&gameKey=" + this.gameKey + (query ? "&" + query : "");
                xhr.open("GET", url, false);
                xhr.withCredentials = true;
                xhr.setRequestHeader("Accept", "application/json");
                xhr.send(null);
                if (xhr.status < 200 || xhr.status >= 300) return null;
                return JSON.parse(xhr.responseText);
            } catch (error) {
                console.warn("BigHead 2: server load unavailable", error);
                return null;
            }
        },

        postForm: function (formBody) {
            var options = {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8",
                    "X-BigHead-Request": "1"
                },
                body: formBody,
                keepalive: formBody.length < 60000
            };
            if (typeof fetch === "function") {
                return fetch(this.endpoint, options).then(function (response) {
                    if (!response.ok) throw new Error("HTTP " + response.status);
                    return response.json();
                });
            }
            return new Promise(function (resolve, reject) {
                var xhr = new XMLHttpRequest();
                xhr.open("POST", BigHeadStorage.endpoint, true);
                xhr.withCredentials = true;
                xhr.setRequestHeader("Content-Type", options.headers["Content-Type"]);
                xhr.setRequestHeader("X-BigHead-Request", "1");
                xhr.onreadystatechange = function () {
                    if (xhr.readyState !== 4) return;
                    if (xhr.status >= 200 && xhr.status < 300) {
                        try { resolve(JSON.parse(xhr.responseText)); }
                        catch (error) { reject(error); }
                    } else {
                        reject(new Error("HTTP " + xhr.status));
                    }
                };
                xhr.send(formBody);
            });
        },

        enqueueSave: function (player, envelope) {
            var self = this;
            var queueKey = String(player);
            self.queues[queueKey] = envelope;
            if (self.queues[queueKey + ":running"]) return;
            self.queues[queueKey + ":running"] = true;

            var pump = function () {
                var job = self.queues[queueKey];
                if (!job) {
                    self.queues[queueKey + ":running"] = false;
                    return;
                }
                self.queues[queueKey] = null;
                var body = "action=save&gameKey=" + self.gameKey +
                    "&player=" + encodeURIComponent(player) +
                    "&data=" + encodeURIComponent(job.data) +
                    "&points=" + encodeURIComponent(job.points || 0) +
                    "&shabbatPoints=" + encodeURIComponent(job.shabbatPoints || 0) +
                    "&hanukkaPoints=" + encodeURIComponent(job.hanukkaPoints || 0) +
                    "&purimPoints=" + encodeURIComponent(job.purimPoints || 0);
                self.postForm(body).then(function (response) {
                    if (response && response.success) {
                        var current = self.readEnvelope(player);
                        if (current && current.savedAt === job.savedAt) {
                            current.dirty = false;
                            current.legacy = false;
                            self.writeEnvelope(player, current);
                        }
                    }
                    pump();
                }).catch(function (error) {
                    console.warn("BigHead 2: server save deferred; local copy is safe", error);
                    self.queues[queueKey + ":running"] = false;
                });
            };
            pump();
        },

        localPlayerCount: function () {
            var count = 0;
            for (var player = 0; player < 4; player++) {
                if (this.readEnvelope(player)) count++;
            }
            return count;
        },

        serverEnvelope: function (server) {
            var data = this.parseData(server && server.data);
            if (!data) return null;
            return {
                data: JSON.stringify(data),
                points: this.pointsFrom(data, server.points),
                shabbatPoints: server.shabbatPoints || 0,
                hanukkaPoints: server.hanukkaPoints || 0,
                purimPoints: server.purimPoints || 0,
                savedAt: Date.now(),
                dirty: false,
                legacy: false
            };
        },

        chooseLocalOverServer: function (local, serverEnvelope) {
            if (!local || !local.dirty || local.legacy) return false;
            var localData = this.parseData(local.data);
            var serverData = this.parseData(serverEnvelope.data);
            var localName = this.normalizedName(localData);
            var serverName = this.normalizedName(serverData);
            if (!localName || !serverName || localName !== serverName) return null;
            return this.pointsFrom(localData, local.points) >
                this.pointsFrom(serverData, serverEnvelope.points);
        }
    },

    loadDataJS__deps: ["$BigHeadStorage"],
    loadDataJS: function (player) {
        var local = BigHeadStorage.readEnvelope(player);
        var server = BigHeadStorage.requestSync("load", "player=" + encodeURIComponent(player));

        if (server && server.success && server.found && server.data) {
            var serverEnvelope = BigHeadStorage.serverEnvelope(server);
            if (serverEnvelope) {
                var choice = BigHeadStorage.chooseLocalOverServer(local, serverEnvelope);
                if (choice === true) {
                    BigHeadStorage.enqueueSave(player, local);
                    return BigHeadStorage.returnString(local.data);
                }
                if (choice === null) {
                    BigHeadStorage.backupConflict(player, local);
                    console.warn("BigHead 2: different local player preserved as a conflict backup; server was loaded");
                }
                BigHeadStorage.writeEnvelope(player, serverEnvelope);
                return BigHeadStorage.returnString(serverEnvelope.data);
            }
        }

        if (local) {
            if (server && server.success && !server.found) {
                local.dirty = true;
                local.legacy = false;
                BigHeadStorage.writeEnvelope(player, local);
                BigHeadStorage.enqueueSave(player, local);
            } else if (local.dirty && !local.legacy) {
                BigHeadStorage.enqueueSave(player, local);
            }
            return BigHeadStorage.returnString(local.data);
        }
        return 0;
    },

    saveDataJS__deps: ["$BigHeadStorage"],
    saveDataJS: function (data, player, points, shabbatPoints, hanukkaPoints, purimPoints) {
        var envelope = {
            data: BigHeadStorage.pointerToString(data),
            points: points,
            shabbatPoints: shabbatPoints,
            hanukkaPoints: hanukkaPoints,
            purimPoints: purimPoints,
            savedAt: Date.now(),
            dirty: true,
            legacy: false
        };
        BigHeadStorage.writeEnvelope(player, envelope);
        BigHeadStorage.enqueueSave(player, envelope);
    },

    getPlayersCountJS__deps: ["$BigHeadStorage"],
    getPlayersCountJS: function () {
        var server = BigHeadStorage.requestSync("count", "");
        var localCount = BigHeadStorage.localPlayerCount();
        if (server && server.success && server.count > 0) return Math.max(server.count, localCount);
        return localCount;
    },

    getLastNameJS__deps: ["$BigHeadStorage"],
    getLastNameJS: function () {
        var userName = (typeof getUserName === "function" ? getUserName() : "") || "";
        return BigHeadStorage.returnString(userName);
    },

    saveWinnersJS__deps: ["$BigHeadStorage"],
    saveWinnersJS: function (winner, loser) {
        var body = "action=result&gameKey=" + BigHeadStorage.gameKey +
            "&winner=" + encodeURIComponent(BigHeadStorage.pointerToString(winner)) +
            "&loser=" + encodeURIComponent(BigHeadStorage.pointerToString(loser));
        BigHeadStorage.postForm(body).catch(function (error) {
            console.warn("BigHead 2: result log was not saved", error);
        });
    },

    pushWinnersJS: function () {
        localStorage.setItem("pushWinners_2", "1");
    }
});
