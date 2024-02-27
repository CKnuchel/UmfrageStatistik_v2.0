using Common.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;

public static class DbContextTestSetup
{
    #region Publics
    /// <summary>
    /// Erstellt eine DbContextOptions-Instanz für UmfrageContext mit einer einzigartigen In-Memory-Datenbank.
    /// Dies ermöglicht isolierte Testumgebungen durch Nutzung unterschiedlicher Datenbanknamen.
    /// </summary>
    /// <returns>Eine konfigurierte DbContextOptions-Instanz für UmfrageContext.</returns>
    public static DbContextOptions<UmfrageContext> CreateUniqueContextOptions()
    {
        // Nutzt einen einzigartigen Namen für jede In-Memory-Datenbank, basierend auf einem neuen GUID.
        DbContextOptions<UmfrageContext> options = new DbContextOptionsBuilder<UmfrageContext>()
                                                   .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                                                   .Options;

        return options;
    }

    /// <summary>
    /// Initialisiert und konfiguriert einen neuen UmfrageContext für Tests, einschließlich der Erstellung
    /// und Befüllung der In-Memory-Datenbank mit Testdaten.
    /// </summary>
    /// <param name="options">Die zu verwendenden DbContextOptions für die Erstellung des Kontexts.</param>
    /// <returns>Ein vollständig initialisierter UmfrageContext bereit für Tests.</returns>
    public static UmfrageContext CreateContext(DbContextOptions options)
    {
        UmfrageContext context = new(options);

        // Stellt sicher, dass eine vorherige In-Memory-Datenbank gleichen Namens gelöscht wird.
        context.Database.EnsureDeleted();

        // Erstellt eine neue In-Memory-Datenbank und initialisiert sie für Tests.
        context.Database.EnsureCreated();

        // Fügt Testdaten hinzu
        CreateTestDatasets(context);

        return context;
    }

    /// <summary>
    /// Bereinigt Ressourcen, indem die In-Memory-Datenbank gelöscht und der Kontext geschlossen wird.
    /// Diese Methode sollte nach Abschluss der Tests aufgerufen werden, um sicherzustellen, dass alle Ressourcen ordnungsgemäß freigegeben werden.
    /// </summary>
    /// <param name="context">Der zu bereinigende Kontext.</param>
    public static void DestroyContext(UmfrageContext? context)
    {
        // Löscht die In-Memory-Datenbank und gibt den Kontext frei.
        context!.Database.EnsureDeleted();
        context.Dispose();
    }
    #endregion

    #region Privates
    private static void CreateTestDatasets(UmfrageContext context)
    {
        // Module
        List<Modul> testModule = new()
                                 {
                                     new Modul { Id = 1, Name = "101 - Webauftritt erstellen und veröffentlichen" },
                                     new Modul { Id = 2, Name = "304 - Einzelplatz-Computer in Betrieb nehmen" },
                                     new Modul { Id = 3, Name = "335 - Mobile-Applikation realisieren" }
                                 };

        context.Module!.AddRange(testModule);

        // Questions
        List<Question> testQuestions = new()
                                       {
                                           new Question { Id = 1, Text = "Waren dir die Handlungsziele bekannt?", Type = 1 },
                                           new Question { Id = 2, Text = "Wie empfandst du die Dauer des Moduls?", Type = 1 },
                                           new Question { Id = 3, Text = "Wie gut kannst du das gelernte bei deiner täglichen Arbeit anwenden?", Type = 2 },
                                           new Question { Id = 4, Text = "Wie gut wurden die Handlungsziele abgedeckt?", Type = 2 }
                                       };

        context.Questions!.AddRange(testQuestions);

        // Answers
        List<Answer> testAnswer = new()
                                  {
                                      // zu Frage 1
                                      new Answer { Id = 1, Question = testQuestions[0], QuestionId = 1, Text = "Ja, alle (Mir waren alle Handlungsziele bekannt)" },
                                      new Answer { Id = 2, Question = testQuestions[0], QuestionId = 1, Text = "Ja, einige (Mir waren nur einige Handlungsziele bekannt)" },
                                      new Answer { Id = 3, Question = testQuestions[0], QuestionId = 1, Text = "Nein, keine (Mir waren keine Handlungsziele bekannt)" },

                                      // zu Frage 2
                                      new Answer { Id = 4, Question = testQuestions[1], QuestionId = 2, Text = "Die Dauer war angemessen." },
                                      new Answer { Id = 5, Question = testQuestions[1], QuestionId = 2, Text = "Die Dauer wurde dem Umfang des Themas nicht gerecht, hätte meiner Meinung nach länger dauern müssen" },
                                      new Answer { Id = 6, Question = testQuestions[1], QuestionId = 2, Text = "Unnötig lange Dauer, Inhalte hätten auch in kürzerer Zeit vermittelt werden können" },

                                      // zu Frage 3
                                      new Answer { Id = 7, Question = testQuestions[2], QuestionId = 3, Text = "Zahlenwert" },

                                      // zu Frage 4
                                      new Answer { Id = 8, Question = testQuestions[3], QuestionId = 4, Text = "Zahlenwert" }
                                  };

        context.Answers!.AddRange(testAnswer);

        // Responses
        List<Response> testResponses = new()
                                       {
                                           // Zu Frage 1
                                           new Response { Id = 1, AnswerId = 1, ModulId = 1, ResponseDate = new DateTime(2016, 08, 23), Value = 1 },
                                           new Response { Id = 2, AnswerId = 1, ModulId = 1, ResponseDate = new DateTime(2017, 04, 02), Value = 1 },
                                           new Response { Id = 3, AnswerId = 2, ModulId = 1, ResponseDate = new DateTime(2018, 08, 02), Value = 1 },
                                           new Response { Id = 4, AnswerId = 3, ModulId = 1, ResponseDate = new DateTime(2016, 08, 01), Value = 1 },
                                           new Response { Id = 5, AnswerId = 2, ModulId = 2, ResponseDate = new DateTime(2017, 05, 12), Value = 1 },
                                           new Response { Id = 6, AnswerId = 3, ModulId = 3, ResponseDate = new DateTime(2017, 05, 01), Value = 1 },

                                           // Zu Frage 2
                                           new Response { Id = 7, AnswerId = 4, ModulId = 2, ResponseDate = new DateTime(2017, 05, 20), Value = 1 },
                                           new Response { Id = 8, AnswerId = 4, ModulId = 1, ResponseDate = new DateTime(2018, 01, 01), Value = 1 },
                                           new Response { Id = 9, AnswerId = 5, ModulId = 3, ResponseDate = new DateTime(2016, 08, 02), Value = 1 },
                                           new Response { Id = 10, AnswerId = 6, ModulId = 1, ResponseDate = new DateTime(2017, 07, 31), Value = 1 },

                                           // zu Frage 3
                                           new Response { Id = 11, AnswerId = 7, ModulId = 2, ResponseDate = new DateTime(2017, 02, 01), Value = 0 },
                                           new Response { Id = 12, AnswerId = 7, ModulId = 2, ResponseDate = new DateTime(2017, 07, 31), Value = 10 },
                                           new Response { Id = 13, AnswerId = 7, ModulId = 2, ResponseDate = new DateTime(2017, 04, 15), Value = 8 },
                                           new Response { Id = 14, AnswerId = 7, ModulId = 3, ResponseDate = new DateTime(2017, 04, 09), Value = 10 },
                                           new Response { Id = 15, AnswerId = 7, ModulId = 1, ResponseDate = new DateTime(2018, 08, 01), Value = 8 },

                                           // zu Frage 4
                                           new Response { Id = 16, AnswerId = 8, ModulId = 2, ResponseDate = new DateTime(2016, 08, 01), Value = 8 },
                                           new Response { Id = 17, AnswerId = 8, ModulId = 3, ResponseDate = new DateTime(2016, 07, 31), Value = 10 },
                                           new Response { Id = 18, AnswerId = 8, ModulId = 3, ResponseDate = new DateTime(2018, 05, 28), Value = 0 },
                                           new Response { Id = 19, AnswerId = 8, ModulId = 1, ResponseDate = new DateTime(2017, 04, 08), Value = 10 },
                                           new Response { Id = 20, AnswerId = 8, ModulId = 1, ResponseDate = new DateTime(2016, 02, 01), Value = 6 }
                                       };

        context.Responses!.AddRange(testResponses);

        context.SaveChanges();
    }
    #endregion
}