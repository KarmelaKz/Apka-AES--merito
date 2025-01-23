# Apka AES     Karolina Kozłowska 159824
Komunikator tekstowy z szyfrowaniem AES i podatność na atak typu MITM (Man-in-the-Middle) i ochrony przed nim. 
1.	Protokół TCP 
TCP zapewnia niezawodną transmisję danych przez sieć, gwarantując, że pakiety dotrą do odbiorcy w odpowiedniej kolejności i bez błędów. Gwarantuje dostarczenie danych, obsługuje kontrolę przepływu żeby nie przeciążać sieci i potwierdza odebranie danych.

2.	Algorytmy szyfrowania AES
Algorytm szyfrowania symetrycznego, działa na blokadach i używa tego samego klucza do szyfrowania i deszyfrowania. AES jest wykorzystywany do szyfrowania wiadomości między klientem a serwerem, zapewniając poufność danych.

Potencjalny atak 
a.	Brute-force – próba złamania szyfru przez sprawdzanie wszystkich kombinacji kluczy
b.	Atak na IV – Jak się niewłaściwie zarządza wektorem inicjalizacji to staje się podatne na ataki

3.	HMAC
Weryfikacja integralności, jest używana do zapewnienia integralności wiadomości i autentyczności nadawcy, jest ona oparta na funkcji skrótu i tajnym kluczu. HMAC jest używany do weryfikacji, że wiadomość przesyłana między klientem a serwerem nie została zmieniona oraz do potwierdzenia autentyczności nadawcy. Wykorzystanie klucza uniemożliwia atak przez samą znajomość funkcji skrótu.

Potencjalny atak 
a.	Na klucz HMAC – przez niewłaściwe przechowywanie, może zostać odkryty i użyty do manipulacji wiadomościami
b.	Replay Attack– Przechwycenie wiadomości i rozesłanie ich jeszcze raz, co wymaga dodatkowej ochrony

4.	Potencjalne ataki na aplikacje
a. Atak Man-in-the-Middle (MITM):
•	Opis: Atakujący może przechwycić i zmodyfikować wiadomości wysyłane między klientem a serwerem. Jeśli komunikacja nie jest odpowiednio zabezpieczona, np. brakuje szyfrowania połączenia, atakujący może zmienić zawartość przesyłanych danych.
•	Jak zabezpieczyć przed atakiem : Użycie bezpiecznego kanału komunikacji, np. TLS (Transport Layer Security), w celu zaszyfrowania całej komunikacji sieciowej.

b. Atak na słabe klucze:
•	Opis: Jeśli klucze AES lub HMAC są zbyt krótkie lub słabe (np. łatwe do odgadnięcia), atakujący może je złamać i uzyskać dostęp do zaszyfrowanych danych.
•	Jak zabezpieczyć przed atakiem : Stosowanie wystarczająco długich kluczy (np. 256-bitowe klucze AES i HMAC) oraz bezpieczne zarządzanie kluczami.

c. Atak na Initialization Vector (IV) :
•	Opis: IV jest używane do zwiększenia losowości szyfrowania. Jeśli IV jest używane niewłaściwie (np. ten sam IV dla różnych wiadomości), może to prowadzić do ujawnienia pewnych informacji o szyfrowanych danych.
•	Jak zabezpieczyć przed atakiem : Zapewnienie, że IV jest losowe i unikalne dla każdej sesji szyfrowania.

