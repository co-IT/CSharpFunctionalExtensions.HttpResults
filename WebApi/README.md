Return Cases:
- Unauthorized
- Forbidden
- Success
- CreatedAtRoute
- UpdatedAtRoute
- ServerError
- BadRequest
- FileResult
- FileStreamResult
- NoContent

Beispiel ErrorOr:
Failure,
Unexpected,
Validation,
Conflict,
NotFound,
Unauthorized,
Forbidden,

Trennen von Domain Errors zu Api Errors (Mapping benötigt)

Fehler als ProblemDetail darstellen


Nutzung von `ProblemDetail` nach [RFC 9457](https://www.rfc-editor.org/rfc/rfc9457). Dies löst `ValidationProblemDetail` nach [RFC 7807](https://www.rfc-editor.org/rfc/rfc7807) ab.

Zur Erstellung evtl. `ProblemDetailsDefaults.Apply(ProblemDetails, statusCode: null);` nutzen.

MimeType: `"application/problem+json"`


ProblemDetails konfigurieren:
- builder.Services.AddProblemDetails();
- IProblemDetailsService verwenden https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-8.0#customize-problem-details



Immer .ToHttpResult am Ende aufrufen
Result --> NoContent / ProblemDetails
Result<T> --> Ok / ProblemDetails
Result<T,E> --> Ok / ProblemDetails (mit custom Mapping)
Spezialfälle:
Result<byte[]> --> FileResult
Result<Stream> --> StreamResult

Alternative Methoden
.ToNoContentResult --> macht aus Result<T> ein NoContent
.ToCreatedAtResult --> man kann noch location bzw. Endpunkt angeben, Created Result mit location header wird erstellt

Mann kann generell immer StatusCode übergeben zum überschreiben

---

Envelope Objekte evtl. unnötig.
Diese enthalten bei uns lediglich 2 Zusatzinfos: TraceId, GeneratedAt

Nachteile:
- Diese Infos sind nur zur Nachverfolgung und werden wahrshcienlich zu 99% nicht im Client ausgewertet. Deshalb sollten sie nicht in den Body.
- Evtl. unübersichtlich weil man geschachtelte Typangaben hat : Envelope<ReadUserDto>

Stattdessen bei Fehlern einheitlich auf den ProblemDetails Standard setzen.
Bei Erfolg einfach das Objekt in den Body.


---

Wir wollen Results komfortabel in der WebApi nutzen. Hierfür müssen wir Results zu HttpResults mappen.

Gemappt werden müssen die Typen:
- Result
- Result<T>
- Result<T,E>
- UnitResult
- UnitResult<E>

Erfolge sollen auf ein passendes HttpResult gemappt werden.

Für Erfolge in der Regel immer nur der Methodenaufruf `.ToHttpResult()` ausreichen. Z.B.:
- Result --> NoContent
- Result<T> --> Ok
- Result<byte[]> --> FileResult
- Result<Stream> --> StreamResult
Allerdings gibt es auch Fälle wo Zusatzinformationen benötigt werden. Hierfür gibt es extra Methoden. Z.B.:
- Result<T> --> .ToCreatedAtHttpResult (man übergibt zusätzlich Inforamtionen für den location Header)
Zudem sind aus Conveniencegründen extra Methoden nötig, um vom Standardverhalten abzuweichen. Z.B.:
- Result<T> --> .ToNoContentHttpResult (wenn man keinen Body senden möchte)

Fehler sollen immer auf ProblemDetails mit passendem StatusCode gemappt werden.
Bei einem Standard Result mit string als Error gibt es ein vorgefertigtes Mapping.
Wie sollte man allerdings mit Custom Errors umgehen? Wichtig ist, dass API und Domäne getrennt sein sollen, das heißt in einer Entity muss ich bei einer Operation z.B. noch nicht wissen welchen StatusCode ich brauche.
In der Domäne wird mit Domänenfehlern gearbeitet, welche in der API passend gemappt werden.
Nun müsste man für jeden Fehlertyp alle oben aufgezählten Extra Methoden ergänzen --> Viel Aufwand. Zudem können Nutzer der nuget Bibliothek die Methoden nicht so einfach erweitern.
Lösung: SourceGeneration. Man legt einen Fehlertyp inkl. Mapper an und über SourceGeneration werden die passenden Extensionmethoden erzeugt.

Eine Alternative wäre über einen Service zu gehen, der das Mapping übernimmt, dann müsste man aber in quasi jedem Endpunkt den Service Injecten und könnte nicht mit der üblichen Syntax (Extensionmethod arbeiten).





