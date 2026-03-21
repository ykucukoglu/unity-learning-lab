using UnityEngine;
using UnityEngine.UI;

public class StaminaHUD : MonoBehaviour
{
    [Header("Dependencies")]
    private PlayerController player;
    public Image staminaFillImage; 

    [Header("Colors")]
    public Color normalColor = new Color(0.1f, 0.9f, 0.4f);
    public Color lowStaminaColor = Color.red;

    private void Start()
    {
        if (staminaFillImage != null)
        {
            // Çubuğun soldan sağa doğru azalması için pivot'u sola çek
            RectTransform rt = staminaFillImage.rectTransform;
            rt.pivot = new Vector2(0, 0.5f);
            rt.anchoredPosition = Vector2.zero; // Pivot değişince pozisyonu sıfırla
        }
    }

    private void Update()
    {
        // Singleton Instance üzerinden oyuncuyu doğrudan yakala
        if (player == null)
        {
            player = PlayerController.Instance;
            if (player == null) return; // Eğer hala bulunamadıysa, bir sonraki frame'i bekle
        }

        if (staminaFillImage == null) return;

        // Enerji oranını hesapla
        float percentage = player.currentStamina / player.maxStamina;

        // Çubuğun genişliğini ölçekleme (Scale) ile ayarla
        staminaFillImage.rectTransform.localScale = new Vector3(percentage, 1, 1);

        // Renk geçişi
        staminaFillImage.color = Color.Lerp(lowStaminaColor, normalColor, percentage);
    }
}
