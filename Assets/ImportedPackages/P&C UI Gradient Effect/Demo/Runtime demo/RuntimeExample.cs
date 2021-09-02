using UnityEngine;
using PolyAndCode.UI.effect;
using UnityEngine.UI;

/// <summary>
/// Examples:
///uIGradient.gradientStyle = GradientStyle.Linear;
///uIGradient.vertexGradientStyle = VertexGradientStyle.Corners;
///uIGradient.defaultblendMode = DefaultBlendModes.Add
///uIGradient.advancedVertexBlending = true;
///uIGradient.blendMode = BlendModes.ColorBurn;
///uIGradient.bottomLeftColor = color;
///uIGradient.bottomRightColor = color;
/// uIGradient.topLeftColor = color;
/// uIGradient.topRightColor = color;
/// uIGradient.startColor = color;
/// uIGradient.endColor = color;
/// uIGradient.gradient = gradient;
/// uIGradient.angle = angle;
/// uIGradient.offset = new Vector2(offsets, offsets);
/// uIGradient.scale = scale;
/// </summary>
public class RuntimeExample : MonoBehaviour
{
    public GameObject uIGradientGO;
    private UIGradient uIGradient;
    [Header("Texts")]
    public Text uiGradientStateText;
    public Text currentGradientStyleText, currentVertexGradientStyleText, currentVertexGradientDirectionText, currentBlendModeText, vertexAdvancedblendingtxt;

    public GameObject defaultGradientFields, advancedGradientFields, vertexLinearFields;

    [Header("Buttons")]
    public Button addRemoveGradientBtn;
    public Button changeSpriteBtn, changeGradientStyleBtn, changeVertexGradientstyleBtn, changeVertexGradientDirBtn, changeBlendModeBtn;
    public Button randomiseColosBtn, randomiseGradientBtn;
    public Toggle vertexAdvancedblendingBtn;

    [Header("Source Images to test blending")]
    public Sprite[] sprites;

    [Header("Vertex Gradient - Corner")]
    public Color topLeft;
    public Color topRight, bottomLeft, bottomRight;

    [Header("Vertex Gradient - Linear")]
    public Color startColor;
    public Color endColor;

    [Header("Gradient")]
    public Gradient gradient;

    [Header("Transformations")]
    public Slider angleSlider;
    public Slider offsetXSlider, offsetYSlider;
    public Slider scaleSlider;
    public Slider opacitySlider;


    int currentSpriteIndex = 0;

    private void Start()
    {
        uIGradient = uIGradientGO.GetComponent<UIGradient>();

        addRemoveGradientBtn.onClick.AddListener(ToggleGradientComponent);
        changeSpriteBtn.onClick.AddListener(ChangeSprite);
        changeGradientStyleBtn.onClick.AddListener(ChangeGradientStyle);
        changeVertexGradientstyleBtn.onClick.AddListener(ChangeVertexGradientstyle);
        changeBlendModeBtn.onClick.AddListener(ChangeBlendMode);
        changeVertexGradientDirBtn.onClick.AddListener(ChangeVertexGradientDirection);
        vertexAdvancedblendingBtn.onValueChanged.AddListener(ToggleVertexBlending);
        randomiseColosBtn.onClick.AddListener(RandomiseColor);
        randomiseGradientBtn.onClick.AddListener(RandomiseGradient);

        angleSlider.minValue = 0;
        angleSlider.maxValue = 360;

        offsetXSlider.minValue = 0;
        offsetXSlider.maxValue = 10;

        offsetYSlider.minValue = 0;
        offsetYSlider.maxValue = 10;

        scaleSlider.minValue = 0;
        scaleSlider.maxValue = 10;

        opacitySlider.minValue = 0;
        opacitySlider.maxValue = 1;

        if (uIGradient != null)
        {
            defaultGradientFields.SetActive((uIGradient.gradientStyle == GradientStyle.Vertex));
            advancedGradientFields.SetActive(!(uIGradient.gradientStyle == GradientStyle.Vertex));
            vertexLinearFields.SetActive(uIGradient.vertexGradientStyle == VertexGradientStyle.Linear);
            vertexAdvancedblendingBtn.isOn = uIGradient.advancedVertexBlending;
        }

    }

    void Update()
    {
        if (uIGradient != null)
        {
            currentGradientStyleText.text = "Gradient Style : " + uIGradient.gradientStyle.ToString();
            currentVertexGradientStyleText.text = "Vertex Gradient Style : " + uIGradient.vertexGradientStyle.ToString();

            if (uIGradient.gradientStyle == GradientStyle.Vertex && !uIGradient.advancedVertexBlending)
            {
                currentBlendModeText.text = "Default Blend Mode : " + uIGradient.defaultblendMode.ToString();
            }
            else
            {
                currentBlendModeText.text = "Advanced Blend Mode : " + uIGradient.blendMode.ToString();
            }

            //vertexAdvancedblendingtxt.text = "Vertex Advanced blending: " + uIGradient.advancedVertexBlending.ToString();
            currentVertexGradientDirectionText.text = "Vertex Gradient direction: " + uIGradient.vertexGradientDirection.ToString();

            uIGradient.opacity = opacitySlider.value;
            uIGradient.defaultModeOpacity = opacitySlider.value;

            //Vertex Gradient - Corner
            uIGradient.bottomLeftColor = bottomLeft;
            uIGradient.bottomRightColor = bottomRight;
            uIGradient.topLeftColor = topLeft;
            uIGradient.topRightColor = topRight;

            //Vertex Gradient - Linear
            uIGradient.startColor = startColor;
            uIGradient.endColor = endColor;

            //Gradient
            uIGradient.gradient = gradient;

            //Transformations
            uIGradient.angle = angleSlider.value;
            uIGradient.offset = new Vector2(offsetXSlider.value, offsetYSlider.value);
            uIGradient.scale = scaleSlider.value;
        }
    }

    /// <summary>
    /// Example:
    ///uIGradient.gradientStyle = GradientStyle.Linear;
    /// </summary>
    private void ChangeGradientStyle()
    {
        if (uIGradient == null)
        {
            return;
        }

        int currentGradientStyle = (int)uIGradient.gradientStyle;
        currentGradientStyle = currentGradientStyle + 1 > 5 ? 0 : currentGradientStyle + 1;

        uIGradient.gradientStyle = (GradientStyle)currentGradientStyle;

        defaultGradientFields.SetActive((uIGradient.gradientStyle == GradientStyle.Vertex));
        advancedGradientFields.SetActive(!(uIGradient.gradientStyle == GradientStyle.Vertex));
    }

    /// <summary>
    /// Example:
    ///uIGradient.vertexGradientStyle = VertexGradientStyle.Corners;
    /// </summary>
    private void ChangeVertexGradientstyle()
    {
        if (uIGradient == null)
        {
            return;
        }
        int vertexGradientIndex = 1 - (int)uIGradient.vertexGradientStyle;

        uIGradient.vertexGradientStyle = (VertexGradientStyle)vertexGradientIndex;
        vertexLinearFields.SetActive(uIGradient.vertexGradientStyle == VertexGradientStyle.Linear);

    }

    /// <summary>
    /// Example:
    ///uIGradient.vertexGradientDirection = VertexGradientDirection.Horizonatal;
    /// </summary>
    private void ChangeVertexGradientDirection()
    {
        if (uIGradient == null)
        {
            return;
        }
        int vertexGradientdir = 1 - (int)uIGradient.vertexGradientDirection;

        uIGradient.vertexGradientDirection = (VertexGradientDirection)vertexGradientdir;

    }

    /// <summary>
    /// Example:
    /// uIGradient.blendMode = BlendModes.ColorBurn;
    /// </summary>
    private void ChangeBlendMode()
    {
        if (uIGradient.gradientStyle == GradientStyle.Vertex && !uIGradient.advancedVertexBlending)
        {
            ChangeVertexBlendMode();
        }
        else
        {
            ChangeAdvancedBlendMode();
        }
    }

    private void ChangeAdvancedBlendMode()
    {
        if (uIGradient == null)
        {
            return;
        }
        int currentBlendMode = (int)uIGradient.blendMode;
        currentBlendMode = (int)uIGradient.blendMode + 1 > 21 ? 0 : currentBlendMode + 1;
        uIGradient.blendMode = (BlendModes)currentBlendMode;
    }

    /// <summary>
    /// Example:
    /// uIGradient.defaultblendMode = DefaultBlendModes.Add
    /// </summary>
    private void ChangeVertexBlendMode()
    {
        if (uIGradient == null)
        {
            return;
        }
        int currentVertexBlendMode = (int)uIGradient.defaultblendMode;
        currentVertexBlendMode = currentVertexBlendMode + 1 > 2 ? 0 : currentVertexBlendMode + 1;

        uIGradient.defaultblendMode = (DefaultBlendModes)currentVertexBlendMode;
    }

    /// <summary>
    /// Example:
    ///   uIGradient.advancedVertexBlending = true;
    /// </summary>
    private void ToggleVertexBlending(bool status)
    {
        if (uIGradient == null)
        {
            return;
        }

        uIGradient.advancedVertexBlending = status;
    }


    /// <summary>
    /// Adds removes gradient component
    /// </summary>
    private void ToggleGradientComponent()
    {
        if (uIGradientGO.GetComponent<UIGradient>() == null)
        {
            uIGradientGO.AddComponent<UIGradient>();
            uIGradient = uIGradientGO.GetComponent<UIGradient>();
            defaultGradientFields.SetActive((uIGradient.gradientStyle == GradientStyle.Vertex));
            advancedGradientFields.SetActive(!(uIGradient.gradientStyle == GradientStyle.Vertex));
            vertexLinearFields.SetActive(uIGradient.vertexGradientStyle == VertexGradientStyle.Linear);
        }
        else
        {
            Destroy(uIGradientGO.GetComponent<UIGradient>());
        }
    }

    /// <summary>
    /// Changes sprite of gradient component
    /// </summary>
    private void ChangeSprite()
    {
        currentSpriteIndex = currentSpriteIndex + 1 > sprites.Length - 1 ? 0 : currentSpriteIndex + 1;
        uIGradientGO.GetComponent<Image>().sprite = sprites[currentSpriteIndex];
    }

    /// <summary>
    /// assing random colors
    /// </summary>
    public void RandomiseColor()
    {
        bottomLeft = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        bottomRight = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        topLeft = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        topRight = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        startColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        endColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    /// <summary>
    /// Assing Random gradient
    /// </summary>
    public void RandomiseGradient()
    {
        gradient = new Gradient();
        int Length = Random.Range(3, 6);
        GradientColorKey[] colorKeys = new GradientColorKey[Length];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[Length];
        Color[] colors = new Color[Length];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }

        float[] times = new float[Length];
        float startTime = 1 / (float)Length;
        for (int i = 0; i < times.Length; i++)
        {
            times[i] = startTime * i;
        }

        for (int i = 0; i < Length; i++)
        {
            colorKeys[i].color = colors[i];
            colorKeys[i].time = times[i];
            alphaKeys[i].alpha = 1.0F;
        }
        gradient.SetKeys(colorKeys, alphaKeys);
    }

}
