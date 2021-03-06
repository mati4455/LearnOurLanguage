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

### Budowanie i tworzenie front-endu
Lokalizacja projektu FrontEndu: `src\LearnOurLanguage.Front`.

Aby zbudować nasz front musimy pobrać NodeJs. Najnowsza wersja aplikacji do budowania używa zalecanego `angular cli`.

Przy pierwszym budowaniu należy uruchomić polecenie `npm install`. 

Do zbudowania frontu wystarczy polecenie `npm run build`.

Przy codziennej pracy uruchamiamy `npm start`.

## Adresy
Serwer: localhost na porcie 5000

Front: localhost na porcie 4200

Czyli po nowemu uruchamiamy stronkę na porcie 4200, a nie jak poprzednio 5000.

