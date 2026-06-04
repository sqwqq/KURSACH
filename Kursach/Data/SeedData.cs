using Kursach.Data;
using Kursach.Models;

namespace Kursach.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Athletes.Any())
            return;

        // Colleges
        var colleges = new List<College>
        {
            new College { Name = "Полоцкий государственный экономический колледж", City = "Полоцк" },
            new College { Name = "Витебский государственный колледж", City = "Витебск" }
        };
        context.Colleges.AddRange(colleges);
        context.SaveChanges();

        // Teams
        var teams = new List<Team>
        {
            new Team { Name = "Баскетбольная команда", SportType = "Баскетбол", CoachName = "Иванов Сергей Петрович", Description = "Чемпионы города 2024", CollegeId = colleges[0].Id },
            new Team { Name = "Волейбольная команда", SportType = "Волейбол", CoachName = "Петрова Анна Михайловна", Description = "Призеры областных соревнований", CollegeId = colleges[0].Id },
            new Team { Name = "Футбольная команда", SportType = "Футбол", CoachName = "Сидоров Алексей Николаевич", Description = "Сборная ПГЭК", CollegeId = colleges[0].Id },
            new Team { Name = "Команда по плаванию", SportType = "Плавание", CoachName = "Козлова Мария Ивановна", Description = "Сборная команда по плаванию", CollegeId = colleges[0].Id },
            new Team { Name = "Легкоатлетическая команда", SportType = "Легкая атлетика", CoachName = "Михайлов Дмитрий Сергеевич", Description = "Команда по легкой атлетике", CollegeId = colleges[0].Id },
            new Team { Name = "Теннисная команда", SportType = "Теннис", CoachName = "Федорова Елена Викторовна", Description = "Секция тенниса", CollegeId = colleges[0].Id }
        };
        context.Teams.AddRange(teams);
        context.SaveChanges();

        // Athletes (12+)
        var athletes = new List<Athlete>
        {
            new Athlete { FirstName = "Александр", LastName = "Козлов", Group = "ИС-21", PhotoUrl = "/images/athletes/Aleksandr_Kozlov.jpg", Bio = "Капитан баскетбольной команды. Играет на позиции разыгрывающего защитника.", Height = 185, Weight = 78, TeamId = teams[0].Id },
            new Athlete { FirstName = "Мария", LastName = "Смирнова", Group = "П-22", PhotoUrl = "/images/athletes/Mariya_Smirnova.jpg", Bio = "Центральная нападающая. Чемпионка города по баскетболу.", Height = 175, Weight = 65, TeamId = teams[0].Id },
            new Athlete { FirstName = "Дмитрий", LastName = "Волков", Group = "ТМ-21", PhotoUrl = "/images/athletes/Dmitriy_Volkov.jpg", Bio = "Связующий игрок. Опыт игры 5 лет.", Height = 190, Weight = 82, TeamId = teams[1].Id },
            new Athlete { FirstName = "Екатерина", LastName = "Новикова", Group = "Э-23", PhotoUrl = "/images/athletes/Ekaterina_Novikova.jpg", Bio = "Либеро. Одна из лучших защитниц в регионе.", Height = 168, Weight = 58, TeamId = teams[1].Id },
            new Athlete { FirstName = "Игорь", LastName = "Морозов", Group = "МЧ-20", PhotoUrl = "/images/athletes/Igor_Morozov.jpg", Bio = "Нападающий. Бомбардир команды.", Height = 182, Weight = 75, TeamId = teams[2].Id },
            new Athlete { FirstName = "Анна", LastName = "Петрова", Group = "П-22", PhotoUrl = "/images/athletes/Anna_Petrova.jpg", Bio = "Пловец. Призер всероссийских соревнований.", Height = 170, Weight = 62, TeamId = teams[3].Id },
            new Athlete { FirstName = "Сергей", LastName = "Кузнецов", Group = "ИС-21", PhotoUrl = "/images/athletes/Sergey_Kuznetsov.jpg", Bio = "Бегун на короткие дистанции. Рекордсмен ПГЭК.", Height = 178, Weight = 72, TeamId = teams[4].Id },
            new Athlete { FirstName = "Ольга", LastName = "Васильева", Group = "ТМ-23", PhotoUrl = "/images/athletes/Olga_Vasileva.jpg", Bio = "Метатель копья. Чемпионка области.", Height = 172, Weight = 66, TeamId = teams[4].Id },
            new Athlete { FirstName = "Никита", LastName = "Степанов", Group = "МЧ-22", PhotoUrl = "/images/athletes/Nikita_Stepanov.jpg", Bio = "Теннисист. Призер региональных турниров.", Height = 175, Weight = 70, TeamId = teams[5].Id },
            new Athlete { FirstName = "Алиса", LastName = "Дмитриева", Group = "Э-21", PhotoUrl = "/images/athletes/Alisa_Dmitrieva.jpg", Bio = "Волейболистка. Игрок основного состава.", Height = 172, Weight = 64, TeamId = teams[1].Id },
            new Athlete { FirstName = "Владимир", LastName = "Егоров", Group = "П-21", PhotoUrl = "/images/athletes/Vladimir_Egorov.jpg", Bio = "Пловец. Специалист на дистанции 100м вольным стилем.", Height = 180, Weight = 75, TeamId = teams[3].Id },
            new Athlete { FirstName = "Полина", LastName = "Зайцева", Group = "ИС-22", PhotoUrl = "/images/athletes/Polina_Zaytseva.jpg", Bio = "Баскетболистка. Снайпер команды.", Height = 170, Weight = 60, TeamId = teams[0].Id }
        };
        context.Athletes.AddRange(athletes);
        context.SaveChanges();

        // Achievements (20+)
        var achievements = new List<Achievement>
        {
            new Achievement { Title = "Чемпионат города по баскетболу", Date = new DateTime(2024, 3, 15), SportType = "Баскетбол", Place = "1 место", AthleteId = athletes[0].Id, TeamId = teams[0].Id },
            new Achievement { Title = "Первенство района по баскетболу", Date = new DateTime(2024, 1, 20), SportType = "Баскетбол", Place = "2 место", AthleteId = athletes[1].Id, TeamId = teams[0].Id },
            new Achievement { Title = "Кубок области по волейболу", Date = new DateTime(2024, 4, 10), SportType = "Волейбол", Place = "1 место", AthleteId = athletes[2].Id, TeamId = teams[1].Id },
            new Achievement { Title = "Волейбольный турнир", Date = new DateTime(2024, 2, 28), SportType = "Волейбол", Place = "3 место", AthleteId = athletes[3].Id, TeamId = teams[1].Id },
            new Achievement { Title = "Чемпионат ПГЭК по плаванию", Date = new DateTime(2024, 5, 5), SportType = "Плавание", Place = "1 место", AthleteId = athletes[5].Id, TeamId = teams[3].Id },
            new Achievement { Title = "Областные соревнования по легкой атлетике", Date = new DateTime(2024, 4, 22), SportType = "Легкая атлетика", Place = "2 место", AthleteId = athletes[6].Id, TeamId = teams[4].Id },
            new Achievement { Title = "Первенство области по метанию", Date = new DateTime(2024, 3, 30), SportType = "Легкая атлетика", Place = "1 место", AthleteId = athletes[7].Id, TeamId = teams[4].Id },
            new Achievement { Title = "Городской теннисный турнир", Date = new DateTime(2024, 4, 15), SportType = "Теннис", Place = "1 место", AthleteId = athletes[8].Id, TeamId = teams[5].Id },
            new Achievement { Title = "Кубок города по волейболу", Date = new DateTime(2024, 5, 1), SportType = "Волейбол", Place = "2 место", AthleteId = athletes[9].Id, TeamId = teams[1].Id },
            new Achievement { Title = "Первенство ПГЭК по плаванию", Date = new DateTime(2024, 2, 10), SportType = "Плавание", Place = "1 место", AthleteId = athletes[10].Id, TeamId = teams[3].Id },
            new Achievement { Title = "Чемпионат района по баскетболу", Date = new DateTime(2024, 1, 15), SportType = "Баскетбол", Place = "1 место", AthleteId = athletes[11].Id, TeamId = teams[0].Id },
            new Achievement { Title = "Межрегиональные соревнования", Date = new DateTime(2024, 6, 1), SportType = "Плавание", Place = "3 место", AthleteId = athletes[5].Id, TeamId = teams[3].Id },
            new Achievement { Title = "Областной теннисный турнир", Date = new DateTime(2024, 5, 20), SportType = "Теннис", Place = "2 место", AthleteId = athletes[8].Id, TeamId = teams[5].Id },
            new Achievement { Title = "Спартакиада студентов", Date = new DateTime(2024, 4, 8), SportType = "Легкая атлетика", Place = "1 место", AthleteId = athletes[6].Id, TeamId = teams[4].Id },
            new Achievement { Title = "Кубок области по баскетболу", Date = new DateTime(2024, 3, 25), SportType = "Баскетбол", Place = "2 место", AthleteId = athletes[0].Id, TeamId = teams[0].Id },
            new Achievement { Title = "Первенство города по плаванию", Date = new DateTime(2024, 2, 20), SportType = "Плавание", Place = "1 место", AthleteId = athletes[10].Id, TeamId = teams[3].Id },
            new Achievement { Title = "Волейбольный кубок района", Date = new DateTime(2024, 4, 18), SportType = "Волейбол", Place = "1 место", AthleteId = athletes[2].Id, TeamId = teams[1].Id },
            new Achievement { Title = "Чемпионат ПГЭК по метанию", Date = new DateTime(2024, 5, 12), SportType = "Легкая атлетика", Place = "1 место", AthleteId = athletes[7].Id, TeamId = teams[4].Id },
            new Achievement { Title = "Турнир по настольному теннису", Date = new DateTime(2024, 3, 5), SportType = "Теннис", Place = "3 место", AthleteId = athletes[8].Id, TeamId = teams[5].Id },
            new Achievement { Title = "Фестиваль спорта", Date = new DateTime(2024, 6, 10), SportType = "Баскетбол", Place = "1 место", AthleteId = athletes[11].Id, TeamId = teams[0].Id }
        };
        context.Achievements.AddRange(achievements);
        context.SaveChanges();

        // News (10+)
        var news = new List<News>
        {
            new News { Title = "Баскетбольная команда выиграла чемпионат города", Content = "ПГЭК одержал победу на городском чемпионате по баскетболу. В финальном матче команда встретилась с колледжем №15 и одержала уверенную победу со счетом 78:65. Особо отличился капитан команды Александр Козлов, набравший 28 очков. Поздравляем наших спортсменов с заслуженной победой!", Date = new DateTime(2024, 5, 20), ImageUrl = "https://placehold.co/800x400/1e3c72/white?text=Basketball+Champions", Category = "Спорт", IsFeatured = true, AthleteId = athletes[0].Id },
            new News { Title = "Новый тренер по плаванию", Content = "Мы рады сообщить о приходе нового тренера по плаванию - Козловой Марии Ивановны. Мария Ивановна имеет звание мастера спорта и более 10 лет опыта работы с юными спортсменами. Запись на тренировки уже открыта!", Date = new DateTime(2024, 5, 18), ImageUrl = "https://placehold.co/800x400/3498db/white?text=Swimming+Coach", Category = "Новости", IsFeatured = false, AthleteId = athletes[5].Id },
            new News { Title = "Волейболисты завоевали кубок области", Content = "Наша волейбольная команда стала победителем областного кубка! В напряженном финальном матче команда одержала победу со счетом 3:2. Связующий игрок Дмитрий Волков был признан MVP турнира.", Date = new DateTime(2024, 5, 15), ImageUrl = "https://placehold.co/800x400/9b59b6/white?text=Volleyball+Win", Category = "Спорт", IsFeatured = true, AthleteId = athletes[2].Id },
            new News { Title = "Открытие нового спортивного зала", Content = "В ПГЭК открылся новый современный спортивный зал площадью 500 квадратных метров. Зал оснащен новейшим оборудованием для занятий баскетболом, волейболом и мини-футболом. Ждем всех желающих на тренировки!", Date = new DateTime(2024, 5, 10), ImageUrl = "https://placehold.co/800x400/27ae60/white?text=New+Gym", Category = "События", IsFeatured = false, AthleteId = null },
            new News { Title = "Легкоатлеты показали отличные результаты", Content = "Наши легкоатлеты приняли участие в областных соревнованиях и завоевали 3 золотые медали. Особенно порадовала Ольга Васильева, занявшая первое место в метании копья.", Date = new DateTime(2024, 5, 8), ImageUrl = "https://placehold.co/800x400/e67e22/white?text=Athletics", Category = "Спорт", IsFeatured = false, AthleteId = athletes[7].Id },
            new News { Title = "День здоровья в ПГЭК", Content = "В субботу в ПГЭК прошел традиционный День здоровья. Студенты соревновались в различных спортивных мероприятиях, а победители получили ценные призы. Благодарим всех участников!", Date = new DateTime(2024, 5, 1), ImageUrl = "https://placehold.co/800x400/1abc9c/white?text=Health+Day", Category = "События", IsFeatured = false, AthleteId = null },
            new News { Title = "Теннисисты завоевали награды", Content = "Наши теннисисты приняли участие в городском турнире и завоевали 2 золотые и 1 серебряную медаль. Никита Степанов стал победителем в одиночном разряде!", Date = new DateTime(2024, 4, 25), ImageUrl = "https://placehold.co/800x400/34495e/white?text=Tennis+Tournament", Category = "Спорт", IsFeatured = false, AthleteId = athletes[8].Id },
            new News { Title = "Пловцы готовятся к чемпионату", Content = "Сборная команда по плаванию активно готовится к областному чемпионату. Тренировки проходят ежедневно в новом спортивном зале. Желаем нашим спортсменам удачи!", Date = new DateTime(2024, 4, 20), ImageUrl = "https://placehold.co/800x400/16a085/white?text=Swimming+Training", Category = "Новости", IsFeatured = false, AthleteId = athletes[5].Id },
            new News { Title = "Спортивные достижения студентов", Content = "Подводим итоги спортивного сезона: наши спортсмены завоевали более 30 медалей на различных соревнованиях. Это лучший результат за последние 5 лет!", Date = new DateTime(2024, 4, 15), ImageUrl = "https://placehold.co/800x400/f39c12/white?text=Sports+Achievements", Category = "События", IsFeatured = false, AthleteId = null },
            new News { Title = "Новая спортивная секция по теннису", Content = "С радостью сообщаем об открытии новой секции по теннису для студентов ПГЭК. Занятия будут проходить под руководством опытного тренера Федоровой Елены Викторовны.", Date = new DateTime(2024, 4, 10), ImageUrl = "https://placehold.co/800x400/8e44ad/white?text=Tennis+Section", Category = "Новости", IsFeatured = false, AthleteId = athletes[8].Id }
        };
        context.News.AddRange(news);
        context.SaveChanges();

        // Competitions (seed only if empty)
        if (!context.Competitions.Any())
        {
            var competitions = new List<Competition>
            {
                new Competition { Title = "Чемпионат города по баскетболу", Date = new DateTime(2026, 6, 15), Location = "Спорткомплекс ГЦОР", SportType = "Баскетбол", Description = "Финал чемпионата города среди колледжей", IsCompleted = false, TeamId = teams[0].Id },
                new Competition { Title = "Кубок области по волейболу", Date = new DateTime(2026, 7, 1), Location = "Витебск, СК Витебск", SportType = "Волейбол", Description = "Областной кубок по волейболу среди студентов", IsCompleted = false, TeamId = teams[1].Id },
                new Competition { Title = "Турнир по футболу", Date = new DateTime(2026, 5, 30), Location = "Стадион ПГЭК", SportType = "Футбол", Description = "Товарищеский матч между колледжами", IsCompleted = false, TeamId = teams[2].Id },
                new Competition { Title = "Спартакиада ПГЭК", Date = new DateTime(2026, 9, 10), Location = "Спортзал ПГЭК", SportType = "Плавание", Description = "Ежегодная спартакиада колледжа", IsCompleted = false, TeamId = teams[3].Id },
                new Competition { Title = "Легкоатлетический кросс", Date = new DateTime(2026, 4, 20), Location = "Городской парк", SportType = "Легкая атлетика", Description = "Осенний легкоатлетический кросс среди студентов", IsCompleted = true, TeamId = teams[4].Id }
            };
            context.Competitions.AddRange(competitions);
            context.SaveChanges();
        }
    }
}