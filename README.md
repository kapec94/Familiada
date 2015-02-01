FAMILIADA
---------

Projekt napisany do szkoły, na konkurs gry w... Familiadę. Unexpected, huh?

__DISLAIMER__: README is written in polish, which you probably already noticed. If you're not familiar with
this language it wont really help you.

### PLIKI:
 * Familiada.exe - uruchamia cały program - oczywiście musisz najpierw go skompilować.
 * pliki XML - zawierają dane rund. Format pliku jest na tyle prosty, że każda osoba z IQ >90 sobie z nim poradzi.
	 Mimo to, patrz: _FORMAT DANYCH RUND_

__UWAGA__: program zakłada, że wszystkie dane są poprawne. Nie wiem, co się stanie, jak w pliku xml znajdzie błąd.

### DZIAŁANIE:
  Są trzy tryby działania: wybór danych, normalna gra i finał.

#### WYBÓR:
  Wybierasz dane, klikasz 'Wczytaj' i lecisz.
	
#### NORMALNA:
  Przy liście odpowiedzi masz napisaną odpowiedź i punktowanie jej. Przycisk plus odsłania odpowiedź i dodaje
  punkty do puli punków, przycisk minus tylko odsłania.	
  Punkty do drużyny 1/2: przekazuje wszystkie punkty z puli danej drużynie. Nie pomyślałem o opcji cofania,
  więc uważaj, w co klikasz.
  Błąd: dodaje danej drużynie błąd (są one czyszczone przy przejściu do następnego pytania). Patrz wyżej.
  Dalej: ekhm...
  	
#### FINAŁ:
  Tu jest fajowo. Ostrzegam, poćwicz paluszki. Jak prowadząca zacznie zadawać pytania, klikasz "Wprowadź"
  i wpisujesz wraz z odpowiadającym odpowiedzi, hehe. Musisz być szybki. Jak już skończyłeś, klikasz
  Escape albo zamykasz okienko, dane się uzupełnią i możesz poprawić babole jakie zrobiłeś podczas wpisywania.
  Z list obok wybierasz pasujące prawidłowe odpowiedzi i klikasz 'pokaż wyniki' aż wszystko się wylistuje w 
  głównym oknie.
  Jak klikniesz "Następny gracz" po drugim graczu, pokaże się okno z jakimś wynikiem. Nie pamiętam, jakim.
  Jestem leniwy i nie sprawdzę w kodzie. Ale w każdym razie już koniec! Hurra!
		
### FORMAT DANYCH RUND:
  Dane rund są umiejscowione w plikach .xml. Program może wczytać je tylko z obecnego
  working directory [jak to po polsku powiedzieć..?]. **TYLKO STAMTĄD**. Nie miałem czasu na pisanie "Przeglądaj...".

  W skrócie format wygląda tak:

  	<RoundData>
  		<NormalMode>
  			<Question Title="Ile palców ma stonoga?">
  				<Answer Points="69">Dwajścia</Answer>
  				<Answer Points="14">Spierdalaj</Answer>
  				<!-- itd -->
  			</Question>
  			<Question Title="...">
  				<!-- et cetera -->
  			</Question>
  			<!-- Tak do sześciu czy tam pięciu -->
  		</NormalMode>
  		<FinalMode>
  			<!-- To samo co wyżej -->
  		</FinalMode>
  	</RoundData>
	
### UWAGI:
Do prawidłowego działania aplikacji wymagany jest .NET Framework 4. Zazwyczaj jest on zainstalowany na
komputerach z systemem od XP-ka w górę.
Jeśli go nie masz, zainstaluj.
Jeśli go nie masz i czujesz się bezradny, mam dla ciebie link: http://lmgtfy.com/?q=.net+framework+4

Program rozpowszechniam pod licencją **GNU GPL** (http://www.gnu.org/licenses/gpl.html), więc jest to 
oprogramowanie **WOLNE**. Nie wolne, w sensie że nie szybkie, tylko **WOLNE**, bo możesz sobie zrobić z nim, 
co ci do głowy przyjdzie.

My work is done here!

**Copyleft @ 2012-2013**, by Mateusz K., known better as *kapec94*.
