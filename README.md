# LearnOurLanguage
## Wymagania wstępne
### Oprogramowanie
+ Visual Studio 2015 Update 3
+ Visual Studio Code -> [Pobierz tutaj](https://code.visualstudio.com/)
+ .NET Core 1.1 -> [Pobierz tutaj](https://www.microsoft.com/net/download/core#/current)
+ SQL Server 2012
+ Node.js -> [Pobierz tutaj](https://nodejs.org/en/)

### Projekt
Na początku należy utworzyć w folderze `src/LearnOurLanguage.Web` plik `appsettings.json`. 
Wzór pliku znajduje się w tym samym katalogu pod nazwą `appsettings.json.template`

Najważnieszym elementem tutaj jest connection string. Czyli połączenie do naszej bazy danych.
Zaproponowany w pliku connection string zadziała dla lokalnej bazy danych, w której autoryzacja będzie 
ustawiona na `Windows Authentication`

### Test bazy danych - migracja
W projekcie dodana jest migracja `init` - sprawdzająca jedynie poprawność połączenia z bazą danych.

Aby przetestować migrację, należy w `Package Manager Console` (dalej nazywane `PM`) wykonać następujące kroki:
+ Wybrać `Default project` na `src\Model`
+ Wykonać komendę `Update-Database`

### Budowanie front-endu
Aby zbudować nasz front musimy pobrać NodeJs.

Prawdopodobnie Visual Studio odzyska za nas pakiety, jednak ja proponuje odpalić skrypt `scripts/first_build.bat`, 
który pobierze wszystkie moduły Node potrzebne dla naszego projektu. Skrypt należy uruchomić jako Administrator.

Teraz pora na zbudowanie aplikacji. W tym celu będziemy uruchamiać skrypt `scripts/build_front_watch.bat`

Podczas produkcji będziemy używać WebPacka a trybie `watch`. Oznacza to, że będzie on nasłuchiwał zmian w plikach 
i od razu je kompilował.

Dodatkowo w projekcie `LearnOurLanguage.Front` można uruchomić polecenie `npm start` - nasłuchiwanie zmian, `npm build` - zbudowanie w trybie dev, `npm build --watch` - zbudowanie i nasłuchiwanie, `npm build-production` - zbudowanie do produkcji.
Te komendy są przydatne, gdy budujemy aplikacje z PowerShella, np. w `VS Code` w zakładce `Console`.

## Tworzenie front-endu
Do tworzenia frontu będziemy używać Visual Studio Code (w żadnym wypadku pełnego Visual Studio).

Aby rozpocząć pracę z tworzeniem należy uruchomić VS Code, a następnie otworzyć folder -> `src/LearnOurLanguage.Front/src` lub główny folder projektu.



