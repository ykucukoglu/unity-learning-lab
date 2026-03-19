# NİHAİ UNITY SAHNE KURULUM KILAVUZU (1:1 REPLİKA)

Bu kılavuz, projedeki kodların (`LevelBuilder.cs`) ürettiği görsel dünyayı Unity Editörü içerisinde adım adım, manuel olarak inşa etmeniz için tasarlanmıştır. Bu bölümde, sahnenin "ruhu" olan tüm materyal ayarlarını bulacaksınız.

---

## 1. ADIM: MATERYAL LABORATUVARI (HAZIRLIK)

Unity'de projenize başlamadan önce `Assets/Materials` klasöründe aşağıdaki materyalleri (Shader: **Universal Render Pipeline/Lit**) oluşturun. Renkleri girerken Hex kodlarını veya RGB değerlerini birebir kullanın.

### 1.1 Temel Blok Materyalleri
Bu materyaller platformların ana gövdesini oluşturur.

| Materyal Adı | Renk (Hex) | Smoothness | Bölge / Notlar |
| :--- | :--- | :--- | :--- |
| **blockMat_Start** | #6B7085 | 0.02 | Başlangıç: Platform üst yüzeyi (Soğuk Gri/Mavi) |
| **cliffMat_Start** | #333645 | 0.02 | Başlangıç: Platform alt/yan gövdesi |
| **blockMat_Mid** | #93877A | 0.02 | Orta Bölgeler: Platform üst yüzeyi (Toprak/Kahve) |
| **cliffMat_Mid** | #564E47 | 0.02 | Orta Bölgeler: Platform alt/yan gövdesi |
| **blockMat_End** | #A8B8C7 | 0.02 | Bitiş Tapınağı: Platform üst yüzeyi (Aydınlık Gri) |
| **cliffMat_End** | #626B74 | 0.02 | Bitiş Tapınağı: Platform alt/yan gövdesi |
| **brownMat** | #594026 | 0.05 | Ağaç gövdeleri ve ahşap yapılar. |

### 1.2 Doğa ve Bitki örtüsü
| Materyal Adı | Renk (Hex) | Smoothness | Notlar |
| :--- | :--- | :--- | :--- |
| **greenMat** | #598C40 | 0.01 | Çimenler, yosunlar ve ağaç yaprakları. |
| **vineMat** | #407333 | 0.01 | Sarmaşıklar (zincir yapılar). |

### 1.3 Parlayan (Emissive) Materyaller
Bu materyaller için Inspector'da **Emission** kutucuğunu işaretleyin (Enable).

| Materyal Adı | Renk (Hex) | Smoothness | Emission (Intensity) |
| :--- | :--- | :--- | :--- |
| **redGlowMat** | #FF0000 | 0.75 | 18.0 (Lazerler ve Dikenler) |
| **emberMat** | #FF0000 | 0.75 | 12.0 (Uçuşan parçacıklar) |
| **glowBlueMat** | #26D1FF | 0.75 | 4.5 (Bitiş halkaları, Checkpoint) |
| **glowYellowMat**| #FFC726 | 0.75 | 4.5 (Toplanabilir objeler) |

### 1.4 Tehlikeli Lav Materyali (Kritik Ayarlar)
| Materyal Adı | Renk (Hex) | Ayarlar |
| :--- | :--- | :--- |
| **deathZoneMat**| #FF0000 | **Surface Type:** Transparent <br> **Alpha:** %35 <br> **Emission:** Kırmızı (Intensity: 8.0) |

> [!IMPORTANT]
> **deathZoneMat** ayarlarında "Surface Type" kısmını **Transparent** yapmayı unutmayın! Aksi takdirde lavın içini göremezsiniz.

---

## 2. ADIM: PREFAB FABRİKASI (MONTAJ TALİMATI)

Sahneyi hızlı ve düzenli bir şekilde kurmak için 6 temel prefab (şablon) oluşturacağız. Bu parçaları `Assets/Prefabs` klasörüne kaydederek sahneye sürükleyip bırakacağız.

### 2.1 PFB_Platform (Pivot: Alt-Orta)
Orijinal oyundaki renk geçişini sağlamak için **3 farklı varyasyonda** zemin şablonu hazırlayacağız. Pivot: Alt-Orta.

**Varyasyon 1: PFB_Platform_Start (Başlangıç Bölgesi)**
1.  **Ana Obje (Parent):** Hierarchy'de sağ tık: `Create Empty`. Adı: `PFB_Platform_Start`.
    - **Ekstra:** Bu objeye bir **Box Collider** ekleyin. Ayarlar: **Center: (0, 0.5, 0), Size: (1, 1, 1)**. Bu sayede tek bir collider tüm platformu kapsar.
2.  **Alt Obje 1 (Gövde):** `PFB_Platform_Start` içine sağ tık: `3D Object > Cube`.
    - Adı: `Base_Body`, **Position:** (0, 0.425, 0), **Scale:** (1, 0.85, 1)
    - **Material:** `cliffMat_Start`
    - **Not:** Bunun içindeki `Box Collider` bileşenini silebilir veya kapatabilirsiniz.
3.  **Alt Obje 2 (Üst Kapak):** `PFB_Platform_Start` içine sağ tık: `3D Object > Cube`.
    - Adı: `Top_Highlight`, **Position:** (0, 0.925, 0), **Scale:** (1, 0.15, 1)
    - **Material:** `blockMat_Start`
    - **Not:** Bunun içindeki `Box Collider` bileşenini silebilir veya kapatabilirsiniz.

Bu objeyi **Prefabs** klasörüne sürükleyerek prefab haline getirin.

**Varyasyon 2: PFB_Platform_Mid (Orta Bölgeler - Kahve Tonlu)**
- Sahnedeki `PFB_Platform_Start` prefabına sağ tıklayıp **Prefab > Unpack** yapın (veya Ctrl+D ile kopyalayın).
- Adını `PFB_Platform_Mid` olarak değiştirin.
- İçindeki `Base_Body` materyalini `cliffMat_Mid`, `Top_Highlight` materyalini `blockMat_Mid` olarak değiştirin.
- Prefabs klasörüne sürükleyerek bunu da yeni bir prefab yapın.

**Varyasyon 3: PFB_Platform_End (Bitiş Bölgesi - Aydınlık)**
- Aynı işlemi tekrarlayıp bir kopya daha oluşturun.
- Adını `PFB_Platform_End` yapın.
- Gövdeye `cliffMat_End`, kapağa `blockMat_End` verip Prefabs klasörüne kaydedin.

### 2.2 PFB_Tree (Low-Poly Köşeli Ağaç)
> [!IMPORTANT]
> **Görünüm Notu:** `LowPolyFoliage` scriptini eklediğinizde ağacın yaprakları koddaki 2-birimlik orijinal boyutuna döner. Bu yüzden aşağıda küçültülmüş, orijinal `LevelBuilder` değerlerini kullanmalısınız:

1.  **Ana Obje:** Empty Object. Adı: `PFB_Tree`.
2.  **Gövde:** `Trunk` (Cylinder). 
    - **Position:** (0, 0.85, 0), **Scale:** (0.3, 0.85, 0.3), **Material:** `brownMat`.
3.  **Yapraklar (Bake İşlemi):**
    - `PFB_Tree` içine iki tane Sphere ekleyin: `Leaves_Main` ve `Leaves_Detail`.
    - İkisine de `LowPolyFoliage.cs` scriptini ekleyin.
    - **Position ve Scale (Icosphere Uyumlu):**
        - **Leaves_Main:** Pos: (0, 2.2, 0), Scale: (1.9, 1.6, 1.9), Mat: `greenMat`.
        - **Leaves_Detail:** Pos: (0.45, 2.7, -0.35), Scale: (1.15, 1, 1.15), Mat: `greenMat`.

### 2.3 PFB_Vine (Sarmaşık Segmenti)
1.  **Ana Obje:** Empty Object. Adı: `PFB_Vine`.
2.  **Segment:** 3D Cube. **Scale: (0.12, 0.4, 0.12), Mat: vineMat.**
3.  Bu segmentleri dikey olarak uç uca ekleyerek farklı boylarda sarmaşıklar oluşturabilirsiniz.

### 2.4 PFB_GrassClump (Çimen Kümesi)
1.  **Ana Obje:** Empty Object. Adı: `PFB_GrassClump`.
2.  İçine 3-5 adet ince Cube ekleyin. **Scale Y: (0.15 - 0.4 arası), Rotation: (Rastgele hafif eğim), Mat: greenMat.**

### 2.5 PFB_Moss (Yosun Yaması)
1.  Tek bir Cube. **Scale: (0.5, 0.05, 0.5), Mat: greenMat.** Platformların üzerine serpiştirilir.

### 2.6 PFB_Spike (Tehlikeli Diken)
1.  **Ana Obje (Parent):** Empty Object. Adı: `PFB_Spike`.
2.  **Alt Obje 1 (Gövde):** `3D Object > Cylinder`. 
    - Adı: `Spike_Body`
    - **Position:** (0, 0.55, 0), **Scale:** (0.38, 0.55, 0.38), **Material:** `redGlowMat`.
    - **Collider:** Üzerindeki **Capsule Collider** değerlerini şu şekilde güncelleyin: **Radius: 0.2, Height: 1.1**.
3.  **Alt Obje 2 (Işık):** Inner empty object. Adı: `Spike_Light`.
    - **Point Light** ekleyin. **Color: Kırmızı, Intensity: 2, Range: 3.**

### 2.7 PFB_Collectible_Yellow (Toplanabilir Obje)
1.  **Ana Obje (Parent):** Empty Object. Adı: `PFB_Collectible_Yellow`.
    - **Ekstra:** Bu objeye bir **Sphere Collider** ekleyin. **Is Trigger: işaretli (True), Radius: 0.8**.
2.  **Alt Obje 1 (Gövde):** `Core_Sphere`.
    - **Sphere Mesh**, **Scale:** (0.78, 0.78, 0.78), **Material:** `glowYellowMat`.
3.  **Alt Obje 2 (Işık):** `Glow_Light`.
    - **Point Light**. **Color: Sarı, Intensity: 2.8, Range: 6.**
4.  **Alt Obje 3 (Dönen Etiket Sistemi):**
    - `Rotation_Pivot` (Empty): **Pos:** (0, 1.5, 0). Üzerine **Rotator.cs** scriptini ekleyin (`rotationSpeed: 0, 20, 0`).
      - `Label` (TextMesh): **Text:** "COLLECT", **Character Size:** 0.12, **Font Size:** 50, **Style:** Bold.
      - `Label_Shadow` (TextMesh): **Text:** "COLLECT", **Character Size:** 0.12, **Font Size:** 52, **Color:** Siyah (0.6 alpha), **Pos Offset:** (0.04, -0.04, 0.04).

### 2.8 PFB_Checkpoint_Blue (Kontrol Noktası)
1.  **Ana Obje (Parent):** Empty Object. Adı: `PFB_Checkpoint_Blue`.
    - **Ekstra:** Bu objeye bir **Sphere Collider** ekleyin. **Is Trigger: işaretli (True), Radius: 1.0**.
2.  **Alt Obje 1 (Gövde):** `Core_Cube`.
    - ***Kritik Not:* Checkpoint'ler kodda KÜP olarak tanımlanmıştır.**
    - **Cube Mesh**, **Scale:** (0.78, 0.78, 0.78), **Material:** `glowBlueMat`.
3.  **Alt Obje 2 (Işık):** `Glow_Light`.
    - **Point Light**. **Color: Mavi, Intensity: 3.5, Range: 8.**
4.  **Alt Obje 3 (Dönen Etiket Sistemi):**
    - `Rotation_Pivot` (Empty): **Pos:** (0, 1.8, 0). Üzerine **Rotator.cs** scriptini ekleyin.
      - `Label` (TextMesh): **Text:** "CHECKPOINT", **Character Size:** 0.12, **Font Size:** 50, **Style:** Bold.
      - `Label_Shadow` (TextMesh): **Text:** "CHECKPOINT", **Character Size:** 0.12, **Font Size:** 52, **Color:** Siyah (0.6 alpha), **Pos Offset:** (0.04, -0.04, 0.04).

### 2.9 PFB_Final_Goal (Bitiş Tapınağı)
1.  **Ana Obje (Parent):** Empty Object. Adı: `PFB_Final_Goal`.
2.  **Alt Obje 1 (Merkez Çekirdek):** `3D Object > Sphere`.
    - Adı: `Core_Sphere`, **Scale:** (1.2, 1.2, 1.2), **Material:** `glowBlueMat`.
3.  **Alt Obje 2 (İç Halka):** `3D Object > Cylinder`.
    - Adı: `Inner_Ring`, **Pos:** (0, 0, 0), **Scale:** (3.5, 0.05, 3.5), **Material:** `glowBlueMat`.
4.  **Alt Obje 3 (Dış Halka):** `3D Object > Cylinder`.
    - Adı: `Outer_Ring`, **Pos:** (0, 0.7, 0), **Scale:** (4.5, 0.02, 4.5), **Material:** `glowBlueMat`.
5.  **Alt Obje 4 (Yazı):** `Checkpoint_Label` gibi bir text objesi.
    - Adı: `Goal_Label`, **Text:** "FINISH", **Color:** Açık Mavi, **Pos:** (0, 2, 0).
6.  **Alt Obje 5 (Işık):** Adı: `Goal_Light`.
    - **Point Light**. **Intensity: 10, Range: 12, Color: Mavi.**

### 2.10 PFB_Laser_Bar (Lazer Çubuğu)
1.  **Tip:** İnce uzun bir Cube. **Scale: (0.16, 1.5, 0.16), Mat: redGlowMat.**

### 2.11 PFB_Start_Marker (Başlangıç Pedestalı)
1.  **Ana Obje:** Empty Object. Adı: `PFB_Start_Marker`.
2.  **Taban (Cube):** **Scale: (2, 0.4, 2), Mat: blockMat_Start.**
3.  **Köşe Sütunları:** 4 adet küçük Cube. **Scale: (0.3, 0.8, 0.3), Mat: blockMat_Start.** Köşelere dizin.
4.  **Işık:** Hafif bir **Point Light**. **Intensity: 3.5, Range: 6, Color: Mavi.**

### 2.12 PFB_Rock_[Zone] (Köşeli Kaya / Icosphere)
Platformların altına sarkan organik kayalardır. `OriginalPlatformsBuilder` ile otomatik oluşturulur.
1.  **Ana Obje:** Sphere temellidir.
2.  **Bileşen:** Üzerinde **LowPolyFoliage.cs** scripti bulunur.
3.  **Görünüm:** Script sayesinde Unity'nin yuvarlak küresi, tıpkı ağaç yaprakları gibi **Icosphere (Köşeli)** bir yapıya bürünür.
4.  **Varyasyonlar:** Bölgesine göre `cliffMat_Start`, `_Mid` veya `_End` materyallerini kullanır.

---

## 3. ADIM: UI & HUD TASARIMI (ARAYÜZ)

Oyuncunun stamina (enerji) seviyesini ve kontrolleri göreceği ekran arayüzünü (HUD) manuel olarak kurabilir veya hazır prefabı kullanabilirsiniz.

> [!TIP]
> **Hazır Çözüm:** `Assets/Prefabs/PFB_HUD_Canvas` prefabını direkt sahneye sürükleyerek tüm Step 3'ü tek seferde tamamlayabilirsiniz. Manuel kurmak isterseniz aşağıdaki adımları izleyin:

### 3.1 Canvas Kurulumu
1.  Hierarchy panelinde sağ tık: `UI > Canvas`. Adını `HUD_Canvas` yapın.
2.  **Canvas** panelinde: "Render Mode" kısmını **Screen Space - Overlay** yapın.
3.  **Canvas Scaler** panelinde:
    - **UI Scale Mode:** Scale With Screen Size
    - **Reference Resolution:** X: 1920, Y: 1080

### 3.2 HUD Paneli (Arkaplan)
1.  `HUD_Canvas` içine sağ tık: `UI > Image`. Adını `HUD_Panel` yapın.
2.  **RectTransform** ayarları (Sol Alt Köşe):
    - **Anchor:** Bottom-Left
    - **Pivot:** (0, 0)
    - **Pos X:** 40, **Pos Y:** 40
    - **Width:** 420, **Height:** 240
3.  **Image:** Renk olarak koyu bir lacivert/siyah seçin (%90 şeffaflık).

### 3.3 Stamina Bar Sistemi
1.  `HUD_Panel` içerisine bir **Text** ekleyin (Adı: `Stamina_Label`). İçeriğe "STAMINA" yazın.
2.  Bar Arkaplanı: Bir **Image** oluşturun (`Stamina_BG`). Renk: Koyu gri/siyah.
3.  Bar Dolgusu: `Stamina_BG` içerisine bir **Image** daha oluşturun (`Stamina_Fill`). 
    - **Color:** Parlak Yeşil (#19E666)
    - **Image Type:** **Filled**
    - **Fill Method:** Horizontal

### 3.4 Kontrol İpuçları
`HUD_Panel` altına 3 adet **Text** objesi ekleyin ve şu metinleri yazın:
*   "WASD  —  Move"
*   "SPACE  —  Jump"
*   "LSHIFT  —  Dash"

---

## 4. ADIM: IŞIKLANDIRMA VE ATMOSFER (HAVA DURUMU)

Sahnenin o puslu, dramatik ve profesyonel görüntüsünü elde etmek için bu ayarları **Window > Rendering > Lighting** ve **Inspector** panellerinden yapın.

### 4.1 Ana Güneş (Sun)
1.  **Hierarchy**'deki dör ana ışığı seçin (yoksa sağ tık: `Light > Directional Light`).
2.  **Rotation:** X: 46, Y: -32, Z: 0
3.  **Color:** #FFF7C7 (Açık sarı/Krem)
4.  **Intensity:** 1.7
5.  **Shadows:** Soft Shadows

### 4.2 Dolgu Işığı (Fill Light)
Gölgelerin simsiyah olmaması ve "yumuşak" görünmesi için ikinci bir ışık ekleyin:
1.  Hierarchy'de sağ tık: `Light > Directional Light`. Adını `FillLight` yapın.
2.  **Rotation:** X: 30, Y: 150, Z: 0
3.  **Color:** #809EE0 (Açık Mavi/Siyahımsı)
4.  **Intensity:** 0.48
5.  **Shadows:** None (Gölge oluşturmasın)

### 4.3 Sis Ayarları (Atmospheric Fog)
1.  **Window > Rendering > Lighting** panelini açın.
2.  **Environment** sekmesine gidin.
3.  **Fog** kutucuğunu işaretleyin (Enable).
    - **Color:** #1F2638 (Koyu lacivert-gri)
    - **Mode:** Exponential Squared
    - **Density:** 0.008

### 4.4 Kamera Arkaplanı
1.  `Main Camera` objesini seçin.
2.  **Clear Flags:** Solid Color yapın.
3.  **Background:** #1F2638 (Sis rengiyle aynı yapın ki ufuk çizgisi kaybolsun).

---

## 5. ADIM: SAHNE ORGANİZASYONU (HİYERARŞİ DÜZENİ)

Sahneye objeleri yerleştirmeye başlamadan önce hiyerarşiyi (Hierarchy) bölge bazlı klasörlere (Empty Objects) ayırmalıyız. Bu, sahnenin düzenli kalmasını sağlar.

### 5.1 Ana Klasörlerin Oluşturulması
Hierarchy panelinde sağ tık: `Create Empty` yapın ve aşağıdaki yapıyı kurun (hepsinin pozisyonu 0,0,0 olmalı):

*   **LEVEL_ROOT** (Tüm sahne bunun içinde olacak)
    *   **01_Start_Area** (Başlangıç platosu)
    *   **02_Left_Shrine** (Soldaki sütunlu ada)
    *   **03_Stepping_Stones** (Ortadaki atlama taşları)
    *   **04_Death_Zone** (Lav ve lazer bölgesi)
    *   **05_Right_Towers** (Sağdaki kuleler ve orman)
    *   **06_High_Bridge** (Yüksek asma köprü)
    *   **07_Final_Shrine** (Bitiş tapınağı)
    *   **99_Decoration** (Dünya geneline yayılmış otlar ve yosunlar)

### 5.2 İsimlendirme Kuralı
Objelerinizi tablolardaki isimleriyle adlandırın (Örn: `Main_Platform`, `Pillar_C`). Bu sayede ileride bir hata olursa hangi objeyi kontrol edeceğinizi anında bulabilirsiniz.

> [!TIP]
> Bir objeyi sahneye koyduğunuzda, onu Hierarchy panelinde ilgili klasöre sürükleyip bırakmayı unutmayın. Örneğin; tüm dikenleri `04_Death_Zone` içine atın.

---

## 6. ADIM: MASTER KOORDİNAT TABLOLARI (BÖLGE BÖLGE)

Aşağıdaki tablolar, koddaki 1:1 orijinal verilerdir. Objeleri Hierarchy’deki kendi klasörlerine sürüklemeyi unutmayın.

### 6.1 BÖLGE: 01_Start_Area

| Obje Adı | Position (X, Y, Z) | Scale (W, H, D) | Prefab / Tip |
| :--- | :--- | :--- | :--- |
| **Main_Plateau** | (-3, 1, -4) | (10, 1, 10) | PFB_Platform_Mid |
| **Thicker_Body** | (-3, -2, -4) | (10, 3, 10) | PFB_Platform_Mid |
| **Cliff_Underside_1** | (-3, -3.5, -4) | (2, 2, 2) | PFB_Rock_Mid (Scale X,Z artırılabilir) |
| **Cliff_Underside_2** | (-3, -6, -4) | (1.5, 1.5, 1.5) | PFB_Rock_Mid |
| **Left_Ledge_1** | (-8.5, 0, -2.5) | (3, 1, 5) | PFB_Platform_Mid |
| **Left_Ledge_2** | (-9, -2, -3) | (2, 2, 4) | PFB_Platform_Mid |
| **Corner_Jut_1** | (2, -1, -5.5) | (4, 1, 3) | PFB_Platform_Mid |
| **Corner_Jut_2** | (1.5, -3, -6) | (3, 2, 2) | PFB_Platform_Mid |
| **Front_Step_1** | (-4.5, -2, -9.5) | (5, 1, 3) | PFB_Platform_Mid |
| **Front_Step_2** | (-4, -3, -12) | (4, 1, 2) | PFB_Platform_Start |
| **Cinematic_Path** | (-2, -2, -19.5) | (2, 1, 9) | PFB_Platform_Start |
| **Stair_1** | (-2, -1.25, -14.25) | (2, 1, 1.5) | PFB_Platform_Start |
| **Stair_2** | (-2, -0.5, -12.75) | (2, 1, 1.5) | PFB_Platform_Start |
| **Stair_3** | (-2, 0.25, -11.25) | (2, 1, 1.5) | PFB_Platform_Start |
| **Stair_4** | (-2, 1.0, -9.75) | (2, 1, 1.5) | PFB_Platform_Mid |
| **Start_Marker** | (-2, -1.9, -22) | (2, 0.4, 2) | PFB_Start_Marker |
| **Start_Light** | (-2, -0.4, -22) | - | Point Light (Mavi) |

### 6.2 BÖLGE: 02_Left_Shrine

| Obje Adı | Position (X, Y, Z) | Scale (W, H, D) | Prefab / Tip |
| :--- | :--- | :--- | :--- |
| **Main_Pillar** | (-12.5, -6, -2.5) | (5, 10, 5) | PFB_Platform_Mid |
| **Top_Cap** | (-12, 4, -2) | (4, 1, 4) | PFB_Platform_Mid |
| **Deep_Root** | (-13, -7.5, -3) | (2.5, 2.5, 2.5) | PFB_Rock_Mid |
| **Side_Buttress** | (-15, -5, -5) | (2, 5, 2) | PFB_Platform_Mid |
| **Mossy_Overhang** | (-12, 4, -2.5) | (8, 1, 7) | PFB_Platform_Mid |
| **Lower_Step** | (-13.5, 3, -5.5) | (3, 1, 3) | PFB_Platform_Mid |
| **Side_Step** | (-12, 3, -3) | (2, 1, 2) | PFB_Platform_Mid |
| **Pedestal_Base**| (-11.5, 5, -1.5) | (3, 1, 3) | PFB_Platform_Mid |
| **Yellow_Orb_Shrine** | (-12, 6.7, -2) | - | PFB_Collectible_Y |
| **Shrine_Tree** | (-16, 5.5, -6) | - | PFB_Tree |

### 6.3 BÖLGE: 03_Stepping_Stones

| Obje Adı | Position (X, Y, Z) | Scale (W, H, D) | Prefab |
| :--- | :--- | :--- | :--- |
| **Stone_A.1** | (-1, -1, -1) | (4, 1, 4) | PFB_Platform_Mid |
| **Stone_A.2** | (-1.5, -2, -1.5) | (3, 1, 3) | PFB_Platform_Mid |
| **Stone_A.3** | (-1.5, -3, -1.5) | (2, 1, 2) | PFB_Platform_Mid |
| **Stone_B.1** | (3, -2, 2) | (4, 1, 4) | PFB_Platform_Mid |
| **Stone_B.2** | (2.75, -3, 1.75) | (2.5, 1, 2.5) | PFB_Platform_Mid |
| **Stone_B.3** | (2, -3.5, 1) | (1.2, 1.2, 1.2) | PFB_Rock_Mid |
| **Extra_Step_A** | (4, -1.25, 2.5) | (2, 0.5, 2) | SADECE KÜP (blockMat_Mid) |
| **Stone_C.1** | (7.5, -1, 5.5) | (5, 1, 5) | PFB_Platform_Mid |
| **Stone_C.2** | (7.5, -2, 5.5) | (4, 1, 4) | PFB_Platform_Mid |
| **Stone_C.3** | (7.25, -3, 5.25) | (1.5, 1.5, 1.5) | PFB_Rock_Mid |
| **Extra_Step_B** | (8.5, -0.25, 8.5) | (2, 0.5, 2) | SADECE KÜP (blockMat_Mid) |
| **Orb_A** | (4, 0, 2.5) | PFB_Collectible_Y |
| **Orb_B** | (8.5, 1, 8.5) | PFB_Collectible_Y |

### 6.4 BÖLGE: 04_Death_Zone

| Obje Adı | Position (X, Y, Z) | Scale (W, H, D) | Tip / Materyal |
| :--- | :--- | :--- | :--- |
| **LAVA_FLOOR** | (13.5, -5.25, 8) | (11, 1.5, 10)| deathZoneMat |
| **DeathZone_Main**| (13, -5, -1.5) | (10, 6, 7) | PFB_Platform_Mid |
| **DeathZone_Top** | (13, 1, -2) | (9, 1, 6) | PFB_Platform_Mid |
| **DeathZone_Root**| (12.5, -6.5, -2) | (3, 3, 3) | PFB_Rock_Mid |
| **Ledge_1** | (8.5, -1, -0.5) | (3, 2, 3) | PFB_Platform_Mid |
| **Ledge_2** | (11, -3, -4.5) | (4, 2, 3) | PFB_Platform_Mid |
| **Right_Isle_Body**| (15, -4, -6) | (6, 5, 6) | PFB_Platform_Mid |
| **Right_Isle_Root**| (14.5, -5.5, -6.5)| (2.5, 2.5, 2.5) | PFB_Rock_Mid |
| **Right_Isle_Top** | (15, 1, -6) | (5, 1, 5) | PFB_Platform_Mid |
| **Right_Step_1** | (14, -1, -6) | (2, 2, 2) | PFB_Platform_Mid |
| **Right_Step_2** | (14, 2, -6) | (2, 1, 2) | PFB_Platform_Mid |
| **Front_Lip** | (13, 0, 0.5) | (8, 1, 3) | PFB_Platform_Mid |
| **Front_Lip_Root**| (12.5, -2, 1) | (6, 2, 2) | PFB_Platform_Mid |
| **Path_Step_1** | (11, 3, 3.5) | (4, 1, 3) | PFB_Platform_Mid |
| **Path_Step_2** | (13, 2.5, 7.5) | (4, 1, 3) | PFB_Platform_Mid |
| **Checkpoint_Base**| (14, 2, 1.5) | (3, 1, 3) | PFB_Platform_Mid |
| **Pit_Floor** | (13.5, -10, 8) | (11, 4, 10) | PFB_Platform_Mid |
| **Wall_Left** | (7.5, -7, 8) | (1, 4, 12) | PFB_Platform_Mid |
| **Wall_Right** | (19.5, -7, 8) | (1, 4, 12) | PFB_Platform_Mid |
| **Wall_Front** | (13.5, -7, 2.5) | (11, 4, 1) | PFB_Platform_Mid |
| **Wall_Back** | (13.5, -7, 13.5)| (11, 4, 1) | PFB_Platform_Mid |
| **Laser_Bar_1** | (11.5, 4.5, 10) | (0.16, 3, 0.16) | PFB_Laser_Bar |
| **Laser_Bar_2** | (12.0, 4.5, 10) | (0.16, 3, 0.16) | PFB_Laser_Bar |
| **Laser_Bar_3** | (12.5, 4.5, 10) | (0.16, 3, 0.16) | PFB_Laser_Bar |
| **Laser_Bar_4** | (13.5, 4.5, 10) | (0.16, 3, 0.16) | PFB_Laser_Bar |
| **Laser_Bar_5** | (14.0, 4.5, 10) | (0.16, 3, 0.16) | PFB_Laser_Bar |
| **Laser_Bar_6** | (14.5, 4.5, 10) | (0.16, 3, 0.16) | PFB_Laser_Bar |
| **Spike_Row_1** | (9, -5, 4) | +1.2 X aralıkla 7 adet | PFB_Spike |
| **Spike_Row_2** | (9, -5, 6) | +1.3 X aralıkla 6 adet | PFB_Spike |
| **Spike_Row_3** | (9, -5, 8) | +1.2 X aralıkla 7 adet | PFB_Spike |
| **Spike_Row_4** | (9, -5, 10) | +1.4 X aralıkla 5 adet | PFB_Spike |
| **Red_Light_1** | (13, -4, 7) | I: 7, R: 20 | Point Light (Kırmızı) |
| **Red_Light_2** | (10, -4, 10) | I: 5, R: 15 | Point Light (Kırmızı) |
| **Red_Light_3** | (17, -4, 8) | I: 5, R: 14 | Point Light (Kırmızı) |
| **Red_Light_4** | (13, -3, 4) | I: 4, R: 12 | Point Light (Kırmızı) |
| **Death_Zone_Text**| (14, -1, 5) | - | TextMesh |
| **Checkpoint** | (14, 3.7, 1.5) | - | PFB_Checkpoint_B |

### 6.5 BÖLGE: 05_Right_Towers

| Obje Adı | Position (X, Y, Z) | Scale (W, H, D) | Prefab |
| :--- | :--- | :--- | :--- |
| **Tower_1** | (17.5, -7, 11.5) | (5, 13, 5) | PFB_Platform_Mid |
| **Tower_1_Root** | (16, -8, 10) | (3.5, 3.5, 3.5) | PFB_Rock_Mid |
| **Tower_1_Ledge** | (15.5, -1, 9.5) | (3, 1, 3) | PFB_Platform_Mid |
| **Tower_1_Deck** | (16.5, 6, 10.5) | (7, 1, 7) | PFB_Platform_Mid |
| **Tower_2** | (12.5, -3, 14.5) | (5, 9, 5) | PFB_Platform_Mid |
| **Tower_2_Root** | (12, -7, 14) | (3, 3, 3) | PFB_Rock_Mid |
| **Tower_2_Cap** | (12.5, 6, 14.5) | (4, 1, 4) | PFB_Platform_Mid |
| **Tower_2_Tip** | (12.5, 7, 14.5) | (3, 1, 3) | PFB_Platform_Mid |
| **Tower_Tree_1** | (15.5, 7.5, 10) | - | PFB_Tree |
| **Tower_Tree_2** | (15, 6, 15) | - | PFB_Tree |

### 6.6 BÖLGE: 06_High_Bridge

| Obje Adı | Position (X, Y, Z) | Scale (W, H, D) | Prefab |
| :--- | :--- | :--- | :--- |
| **Support_Col_1** | (7, -4, 13) | (2, 10, 2) | PFB_Platform_Mid |
| **Support_Col_2** | (4, -5, 13) | (2, 11, 2) | PFB_Platform_Mid |
| **High_Path_Main**| (11, 6, 12) | (12, 1, 2) | PFB_Platform_Mid |
| **High_Path_2** | (4.5, 5, 12) | (5, 1, 2) | PFB_Platform_Mid |
| **High_Path_Sd_1**| (11, 7.25, 11.2) | (12, 0.5, 0.4) | SADECE KÜP (blockMat_Mid) |
| **High_Path_Sd_2**| (11, 7.25, 12.8) | (12, 0.5, 0.4) | SADECE KÜP (blockMat_Mid) |

### 6.7 BÖLGE: 07_Final_Shrine

| Obje Adı | Position (X, Y, Z) | Scale (W, H, D) | Prefab |
| :--- | :--- | :--- | :--- |
| **Massive_Found** | (-4.5, -10, 14.5) | (7, 17, 7) | PFB_Platform_End |
| **Side_Pillar** | (-7.5, -5, 11.5) | (3, 8, 3) | PFB_Platform_End |
| **Found_Root** | (-5, -11, 14) | (3, 3, 3) | PFB_Rock_End |
| **Step_1** | (-8.5, 0, 14) | (3, 1, 4) | PFB_Platform_End |
| **Step_2** | (-6, 2, 10) | (2, 1, 2) | PFB_Platform_End |
| **Victory_Deck** | (-1, 7, 13) | (12, 1, 6) | PFB_Platform_End |
| **Front_Deck** | (-6, 6, 10.5) | (4, 1, 3) | PFB_Platform_End |
| **Arch_Left** | (-4, 8, 15.5) | (2, 6, 1) | PFB_Platform_End |
| **Rock_Cluster_L1** | (-4.2, 7.5, 15.3) | (1.2, 1.2, 1.2) | PFB_Rock_End |
| **Rock_Cluster_L2** | (-3.8, 6.8, 15.7) | (0.8, 1.5, 0.8) | PFB_Rock_End |
| **Rock_Cluster_L3** | (-4.5, 7.2, 15.5) | (1.0, 1.0, 1.0) | PFB_Rock_End |
| **Arch_Right** | (0, 8, 15.5) | (2, 6, 1) | PFB_Platform_End |
| **Rock_Cluster_R1** | (0.2, 7.5, 15.3) | (1.2, 1.2, 1.2) | PFB_Rock_End |
| **Rock_Cluster_R2** | (-0.2, 6.8, 15.7) | (0.8, 1.5, 0.8) | PFB_Rock_End |
| **Rock_Cluster_R3** | (0.5, 7.2, 15.5) | (1.0, 1.0, 1.0) | PFB_Rock_End |
| **Arch_Top** | (-1.5, 13, 15.5) | (9, 1, 1) | PFB_Platform_End |
| **Core_Base** | (-2, 8, 13) | (4, 1, 4) | PFB_Platform_End |
| **Core_Pedestal** | (-2, 9, 13) | (3, 1, 3) | PFB_Platform_End |
| **Sacred_Core** | (-2.5, 11.8, 12.5)| - | PFB_Final_Goal |
| **FINAL_Light** | (-2.5, 12, 12.5) | I: 10, R: 12 | Point Light (Mavi) |

### 6.8 BÖLGE: 99_Decoration

| Obje Adı | Position (X, Y, Z) | Prefab | Notlar |
| :--- | :--- | :--- | :--- |
| **Grass_1** | (-4, 4, -3) | PFB_GrassClump | Start_Area |
| **Grass_2** | (-7, 2, -5) | PFB_GrassClump | Start_Area |
| **Grass_3** | (-3, 2, -7.5) | PFB_GrassClump | Start_Area |
| **Grass_4** | (-9, 2, -3) | PFB_GrassClump | Start_Area |
| **Moss_1** | (-8.5, 0, -6) | PFB_Moss | Start_Area |
| **Moss_2** | (0.5, 0, -5) | PFB_Moss | Start_Area |
| **Vine_Row_1** | (-7.5, -2, -9.5) | PFB_Vine | +X yönüne doğru 5 adet yan yana |
| **Vine_Row_2** | (-10, -1, -5) | PFB_Vine | +Z yönüne doğru 3 adet arka arkaya |
| **Grass_5** | (-14, 5, -5) | PFB_GrassClump | Left_Shrine |
| **Grass_6** | (-11, 5, -5.5) | PFB_GrassClump | Left_Shrine |
| **Moss_3** | (-15.5, 5, -3) | PFB_Moss | Left_Shrine |
| **Vine_Row_3** | (-16, 4, -6) | PFB_Vine | +X yönüne 5 adet / +Z yönüne 4 adet |
| **Grass_7** | (-2, 0, -2) | PFB_GrassClump | Stepping_Stones |
| **Grass_8** | (5.5, 0, 4) | PFB_GrassClump | Stepping_Stones |
| **Moss_4** | (1.5, -1, 0.5) | PFB_Moss | Stepping_Stones |
| **Grass_9** | (9.5, 2, -4) | PFB_GrassClump | Death_Zone |
| **Grass_10** | (13, 2, -4) | PFB_GrassClump | Death_Zone |
| **Moss_5** | (15, 2, -5) | PFB_Moss | Death_Zone |
| **Vine_Row_4** | (8, -5, -5) | PFB_Vine | +X yönüne 4 adet yan yana |
| **Vine_Row_5** | (8, 1, -5) | PFB_Vine | +Z yönüne 4 adet arka arkaya |
| **Grass_11** | (11, 7, 13) | PFB_GrassClump | Right_Towers |
| **Grass_12** | (14, 7, 8) | PFB_GrassClump | Right_Towers |
| **Moss_6** | (15, 7, 13.5) | PFB_Moss | Right_Towers |
| **Vine_Row_6** | (15, -7, 9) | PFB_Vine | +Z yönüne 3 adet arka arkaya |
| **Vine_Row_7** | (10, -3, 12) | PFB_Vine | +X yönüne 3 adet yan yana |
| **Grass_13** | (-2, 8, 11) | PFB_GrassClump | Final_Shrine |
| **Grass_14** | (1, 8, 11) | PFB_GrassClump | Final_Shrine |
| **Moss_7** | (-5, 8, 10.5) | PFB_Moss | Final_Shrine |
| **Moss_8** | (-7.5, 7, 12) | PFB_Moss | Final_Shrine |
| **Vine_Row_8** | (-7, 7, 10) | PFB_Vine | +X yönüne 6 adet yan yana |
| **Vine_Row_9** | (-7, 7, 16) | PFB_Vine | +X yönüne 4 adet yan yana |
| **Rope_Vine_1**| (6, 6, 11.5) | PFB_Vine | High_Bridge'den aşağı sarkıtılacaktır |
| **Rope_Vine_2**| (3.5, 5, 11.5)| PFB_Vine | High_Bridge'den aşağı sarkıtılacaktır |
| **Rope_Vine_3**| (10, 6, 11.5) | PFB_Vine | High_Bridge'den aşağı sarkıtılacaktır |

---

## 7. ADIM: NİHAİ HİYERARŞİ YAPISI (SRC)

İnşaat bitince sahnenizin Hierarchy paneli (sol taraf) tam olarak şu hiyerarşik yapıda, eksiksiz ve hatasız görünmelidir:

```text
▼ MAIN_SCENE
  ► Sun (Directional Light)
  ► FillLight (Directional Light)
  ► Main Camera
  ▼ HUD_Canvas (UI)
    ▼ HUD_Panel
      ► Stamina_Label (Text)
      ► Stamina_BG (Image)
        ► Stamina_Fill (Image)
      ► Divider (Image)
      ► Hint_Move (Text)
      ► Hint_Jump (Text)
      ► Hint_Dash (Text)
  ▼ LEVEL_ROOT
    ▼ 01_Start_Area
      ► Main_Plateau (PFB_Platform)
      ► Thicker_Body (PFB_Platform)
      ► Cliff_Underside_1 (PFB_Platform)
      ► Cliff_Underside_2 (PFB_Platform)
      ► Left_Ledge_1 (PFB_Platform)
      ► Left_Ledge_2 (PFB_Platform)
      ► Corner_Jut_1 (PFB_Platform)
      ► Corner_Jut_2 (PFB_Platform)
      ► Front_Step_1 (PFB_Platform)
      ► Front_Step_2 (PFB_Platform)
      ► Cinematic_Path (PFB_Platform)
      ► Stair_1 (PFB_Platform)
      ► Stair_2 (PFB_Platform)
      ► Stair_3 (PFB_Platform)
      ► Stair_4 (PFB_Platform)
      ► Start_Marker (PFB_Start_Marker)
      ► Start_Light (Point Light)
    ▼ 02_Left_Shrine
      ► Main_Pillar (PFB_Platform)
      ► Top_Cap (PFB_Platform)
      ► Deep_Root (PFB_Platform)
      ► Side_Buttress (PFB_Platform)
      ► Mossy_Overhang (PFB_Platform)
      ► Lower_Step (PFB_Platform)
      ► Side_Step (PFB_Platform)
      ► Yellow_Orb_Shrine (PFB_Collectible_Y)
      ► Shrine_Tree (PFB_Tree)
    ▼ 03_Stepping_Stones
      ► Stone_A (PFB_Platform)
      ► Stone_B (PFB_Platform)
      ► Extra_Step_A (PFB_Platform)
      ► Stone_C (PFB_Platform)
      ► Extra_Step_B (PFB_Platform)
      ► Orb_A (PFB_Collectible_Y)
      ► Orb_B (PFB_Collectible_Y)
    ▼ 04_Death_Zone
      ► LAVA_FLOOR (LavaBox)
      ► Pit_Walls (PFB_Platform)
      ► Wall_Left/Right/Front/Back (PFB_Platform)
      ► Laser_Column_L/R (PFB_Platform)
      ► Laser_Bar_1 to 6 (PFB_Laser_Bar)
      ► Spike_Rows 1 to 4 (PFB_Spike)
      ► Red_Light_1 to 4 (Point Light)
      ► Death_Zone_Text (TextMesh)
      ► Checkpoint (PFB_Checkpoint_B)
    ▼ 05_Right_Towers
      ► Tower_1 (PFB_Platform)
      ► Tower_2 (PFB_Platform)
      ► Tower_Cap (PFB_Platform)
      ► Bridge_Floor (PFB_Platform)
      ► Tower_Trees (PFB_Tree)
    ▼ 06_High_Bridge
      ► Support_Col_1 (PFB_Platform)
      ► Support_Col_2 (PFB_Platform)
      ► High_Path (PFB_Platform)
    ▼ 07_Final_Shrine
      ► Massive_Found (PFB_Platform)
      ► Victory_Deck (PFB_Platform)
      ► Arch_Left/Right (PFB_Platform)
      ► FINAL_GOAL (PFB_Final_Goal)
    ▼ 99_Decoration
      ► All Global_Grass (PFB_GrassClump)
      ► All Global_Moss (PFB_Moss)
      ► All Global_Vines (PFB_Vine)
```

Tebrikler! Tüm adımları tamamladınız. Sahneniz artık projenin %100 orijinal manuel kopyasıdır.
