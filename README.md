# FileAnalyzer

FileAnalyzer je webová aplikace vytvořená v ASP.NET Web Forms. Aplikace analyzuje soubory ve zvoleném adresáři a sleduje změny mezi jednotlivými analýzami adresáře. Poté informuje uživatele o nově přidaných, upravených, nebo smazaných souborech.

## Funkce programu:

* Načtení seznamu souborů v zadaném adresáři. V případě, že cesta k adresáři není validní, program informuje uživatele.
* Rekurzivní zpracování všech podadresářů
* Porovnání seznamu souborů s přechozím stavem adresáře uloženým v souboru files.json, který se při inicializaci vytvoří a poté je pravidelně aktualizován
* V případě modifikace souborů je číslo verze povýšeno o 1 a dále dojde ke změně hash hodnoty pro detekci změn v souboru.
* Zobrazení výsledků analýzy adresáře ve formě jednoduchých textových zpráv na webové stránce.

## Ukázka programu

![Inicializace adresáře](.FileAnalyzer/FileAnalyzer/README/init.PNG)

## Potenciální omezení aplikace:

* Výkon: Program rekurzivně projde všechny soubory a adresáře, což může být pomalé, pokud je analyzován velký adresář nebo je prováděno mnoho analýz současně.
* Ukládání dat: Aplikace ukládá informace o souborech do jednoho JSON souboru ("files.json"). Při zpracování velkých adresářů může tento soubor rychle narůstat. Pro lepší škálovatelnost by bylo vhodné použít databázi nebo jiný efektivnější způsob ukládání dat.
* Souběžné použití: Aplikace není navržena pro souběžné použití více uživateli. Při současném přístupu může dojít ke konfliktu.
