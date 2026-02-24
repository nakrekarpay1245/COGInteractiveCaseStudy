Kullanıcının tüm kodlama taleplerinde şu kurallar sabit olarak uygulanmalı:

Tüm kodlama görevlerinde, mimarisi ve işlevselliği kıdemli bir geliştirici tarafından tasarlanmış gibi
sağlam, ölçeklenebilir, performanslı ve SOLID ile OOP prensiplerine uygun, üst düzey kaliteli çözümler
üretilmelidir. Ancak yazılan her satır kod, bir junior geliştirici tarafından kolayca okunup anlaşılabilir
olmalıdır.

Kurallar:
- Dışarıdan erişilemeyen değişkenler `private VariableType _variableName` biçiminde tanımlanmalıdır.
- Erişim gerekenler ise private olanların getter ve setter' ı şeklinde ve `public VariableType VariableName { get _privateVariableName; private set _privateVariableName; }` biçiminde kapsüllenmelidir.
- Event/Action gerektiği yerde gerektiği kadar kullanılmalıdır.
- Fonksiyon ve değişken tekrarından kaçınılmalıdır.
- Kullanıcının sağladığı mevcut kodlar asla değiştirilmemelidir.
- Yorum satırı yalnızca çok gerekli durumlarda, kısa ve İngilizce yazılmalıdır.
- Kod yalnızca İngilizce dilinde yazılmalıdır.
- Kod okunurluğu yüksek olmalıdır.
- TriInspector ve Unity'nin sunduğu attribute’lar yalnızca gerektiği yerlerde kullanılmalıdır.
- DOTween ve TriInspector paketleri yalnızca performans, SOLID ve OOP prensipleri ihlal edilmeyecek şekilde  ve gerektiği yerlerde kullanılmalıdır.
- DOTeen kullanılan yerlerde SetAutoKill ve SetLink gibi önlemler alınmalıdır.
- namespace ve klasör yapısına dikkat edilmelidir
- var kullanılmamalı sınıf adı veya değişken tipi ne ise o yazılmalı
- Tüm kod dosyaları Assets/_Game/Scripts altında olsun
Kodlar güçlü ama basit bir yapıya sahip olmalı; ardında derinlik taşımalı, yüzeyde sade ve bakım dostu 
olmalıdır.