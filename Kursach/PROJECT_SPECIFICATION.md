# Спортивный портал колледжа ПГЭК (PGEC Sports)

## 1. ОБЩЕЕ ОПИСАНИЕ

**Название:** PGEC Sports — Спортивный портал Полоцкого государственного экономического колледжа  
**Платформа:** ASP.NET Core MVC (.NET 9)  
**База данных:** SQLite (файл: App_Data/CollegeSports.db)  
**Язык интерфейса:** Русский  

**Назначение:** Публикация и управление спортивными достижениями студентов колледжа.  
Система делится на две зоны:
- **Публичная** — просмотр спортсменов, команд, новостей, достижений, соревнований, рейтинга
- **Административная** — CRUD-управление всеми сущностями, авторизация по логину/паролю (SHA256)

---

## 2. ТЕХНОЛОГИЧЕСКИЙ СТЕК

| Компонент | Технология |
|-----------|-----------|
| Backend | ASP.NET Core MVC (.NET 9) |
| ORM | Entity Framework Core 9 |
| БД | SQLite (Microsoft.Data.Sqlite) |
| Аутентификация | Session-based (SHA256 хеш пароля) |
| Frontend | Bootstrap 5.3, Bootstrap Icons 1.11 |
| Анимации | AOS.js 2.3.1 |
| Графики | Chart.js 4.4 |
| Валидация | jQuery Validation + Unobtrusive |
| Загрузка файлов | IFormFile → wwwroot/uploads/ |

---

## 3. СХЕМА БАЗЫ ДАННЫХ (6 таблиц)

### 3.1 College (Колледж)
| Поле | Тип | Ограничения |
|------|-----|-------------|
| Id | int | PK, auto-increment |
| Name | string | Required, max 200 |
| City | string | Required, max 100 |
| CreatedAt | DateTime | Default UTC now |
| UpdatedAt | DateTime | Default UTC now |

**Связи:** College 1→∞ Team (один колледж — много команд)

### 3.2 Team (Команда)
| Поле | Тип | Ограничения |
|------|-----|-------------|
| Id | int | PK |
| Name | string | Required, 3-200 |
| SportType | string | Required, max 100 |
| CoachName | string | Required, max 200 |
| Description | string | Max 2000 |
| CollegeId | int | FK → College.Id, Restrict delete |
| CreatedAt | DateTime | |
| UpdatedAt | DateTime | |

**Связи:** Team ∞→1 College, Team 1→∞ Athlete, Team 1→∞ Achievement, Team 1→∞ Competition

### 3.3 Athlete (Спортсмен)
| Поле | Тип | Ограничения |
|------|-----|-------------|
| Id | int | PK |
| FirstName | string | Required, 2-100 |
| LastName | string | Required, 2-100 |
| Group | string | Required, max 20 |
| PhotoUrl | string | Optional (URL) |
| Bio | string | Max 1000 |
| Height | double | Range 100-250 |
| Weight | double | Range 30-200 |
| TeamId | int | FK → Team.Id, Restrict delete |
| CreatedAt | DateTime | |
| UpdatedAt | DateTime | |

**Связи:** Athlete ∞→1 Team, Athlete 1→∞ Achievement, Athlete 1→∞ News

### 3.4 Achievement (Достижение)
| Поле | Тип | Ограничения |
|------|-----|-------------|
| Id | int | PK |
| Title | string | Required, max 200 |
| Date | DateTime | Required |
| SportType | string | Required, max 100 |
| Place | string | Required, max 50 (формат: "1 место", "2 место"... ) |
| AthleteId | int | FK → Athlete.Id, SetNull delete |
| TeamId | int? | FK → Team.Id, SetNull delete |
| CreatedAt | DateTime | |
| UpdatedAt | DateTime | |

### 3.5 News (Новость)
| Поле | Тип | Ограничения |
|------|-----|-------------|
| Id | int | PK |
| Title | string | Required, 3-200 |
| Content | string | Required, max 10000 |
| Date | DateTime | Required |
| ImageUrl | string | Optional |
| Category | string | Required, max 50 |
| IsFeatured | bool | Default false |
| AthleteId | int? | FK → Athlete.Id, SetNull delete |
| CreatedAt | DateTime | |
| UpdatedAt | DateTime | |

### 3.6 Competition (Соревнование)
| Поле | Тип | Ограничения |
|------|-----|-------------|
| Id | int | PK |
| Title | string | Required, max 200 |
| Date | DateTime | Required |
| Location | string | Max 200 |
| SportType | string | Required, max 100 |
| Description | string | Max 1000 |
| IsCompleted | bool | Default false |
| TeamId | int? | FK → Team.Id, SetNull delete |
| CreatedAt | DateTime | |

---

## 4. ПОЛНЫЙ СПИСОК МАРШРУТОВ (ROUTES)

### 4.1 Публичные (без авторизации)

| Метод | URL | Контроллер | Action | Назначение |
|-------|-----|------------|--------|------------|
| GET | `/` или `/Home` | Home | Index | Главная страница |
| GET | `/Home/About` | Home | About | О проекте |
| GET | `/Home/Contact` | Home | Contact | Контакты |
| POST | `/Home/SendContact` | Home | SendContact | Отправка сообщения |
| GET | `/Home/Leaderboard` | Home | Leaderboard | Рейтинг спортсменов |
| GET | `/Home/Search` | Home | Search | Глобальный поиск |
| GET | `/Athlete` | Athlete | Index | Список спортсменов |
| GET | `/Athlete/Details/{id}` | Athlete | Details | Карточка спортсмена |
| GET | `/Team` | Team | Index | Список команд |
| GET | `/Team/Details/{id}` | Team | Details | Карточка команды |
| GET | `/Achievement` | Achievement | Index | Все достижения |
| GET | `/News` | News | Index | Все новости |
| GET | `/News/Details/{id}` | News | Details | Новость полностью |
| GET | `/Competition` | Competition | Index | Календарь соревнований |
| GET | `/Competition/Details/{id}` | Competition | Details | Соревнование |
| GET | `/Error` | Error | Index | Страница ошибки |
| GET | `/Error/{code}` | Error | StatusCode | 404, 500 |

### 4.2 Административные (требуют [Authorize])

| Метод | URL | Контроллер | Action |
|-------|-----|------------|--------|
| GET | `/Admin` | Admin | Index |
| GET | `/Admin/Login` | Admin | Login |
| POST | `/Admin/Login` | Admin | Login |
| GET | `/Admin/Logout` | Admin | Logout |
| GET/POST | `/AdminAthlete` | AdminAthlete | Index, Create, Edit, Delete |
| GET/POST | `/AdminAchievement` | AdminAchievement | Index, Create, Edit, Delete |
| GET/POST | `/AdminTeam` | AdminTeam | Index, Create, Edit, Delete |
| GET/POST | `/AdminCollege` | AdminCollege | Index, Create, Edit, Delete |
| GET/POST | `/AdminCompetition` | AdminCompetition | Index, Create, Edit, Delete |
| GET/POST | `/AdminNews` | AdminNews | Index, Create, Edit, Delete |

---

## 5. АВТОРИЗАЦИЯ (ADMIN)

**Тип:** Session-based (не ASP.NET Identity)  
**Хранение учётных данных:** appsettings.json
```json
"Admin": {
    "Login": "admin",
    "PasswordHash": "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk="
}
```
**Пароль (открытый):** admin123  
**Хеш:** SHA256 → Base64  
**Механизм:** Кастомный `[Authorize]` атрибут (ActionFilterAttribute), проверяющий сессию.

---

## 6. СТРУКТУРА ПРОЕКТА

```
Kursach/
├── App_Data/               # SQLite БД (CollegeSports.db)
├── Controllers/            # 12 контроллеров MVC + 1 Error
│   ├── HomeController.cs
│   ├── AthleteController.cs
│   ├── TeamController.cs
│   ├── AchievementController.cs
│   ├── NewsController.cs
│   ├── CompetitionController.cs
│   ├── AdminController.cs
│   ├── AdminAthleteController.cs
│   ├── AdminAchievementController.cs
│   ├── AdminTeamController.cs
│   ├── AdminCollegeController.cs
│   ├── AdminCompetitionController.cs
│   ├── AdminNewsController.cs
│   └── ErrorController.cs
├── Models/                 # 7 моделей
│   ├── Athlete.cs
│   ├── Team.cs
│   ├── College.cs
│   ├── Achievement.cs
│   ├── News.cs
│   ├── Competition.cs
│   └── SearchResult.cs
├── Data/
│   ├── AppDbContext.cs     # EF Core контекст
│   └── SeedData.cs         # Начальные данные (12 спортсменов, 6 команд, 20 достижений...)
├── Views/
│   ├── Home/               # 5 вьюх
│   ├── Athlete/            # 2 вьюхи
│   ├── Team/               # 2 вьюхи
│   ├── Achievement/        # 1 вьюха
│   ├── News/               # 2 вьюхи
│   ├── Competition/        # 1 вьюха
│   ├── AdminAthlete/       # 5 вьюх
│   ├── AdminAchievement/   # 4 вьюхи
│   ├── AdminTeam/          # 4 вьюхи
│   ├── AdminCollege/       # 4 вьюхи
│   ├── AdminCompetition/   # 4 вьюхи
│   ├── AdminNews/          # 5 вьюх
│   ├── Admin/              # 2 вьюхи
│   ├── Shared/             # _Layout.cshtml
│   └── Error/              # 1 вьюха
├── wwwroot/
│   ├── css/
│   ├── js/
│   ├── lib/                # Bootstrap, jQuery
│   ├── images/
│   │   ├── athletes/       # Фото спортсменов (внешние файлы)
│   │   └── sports/         # Фото видов спорта
│   ├── uploads/            # Загруженные через админку файлы
│   ├── sitemap.xml
│   └── robots.txt
├── Program.cs              # Точка входа
└── appsettings.json        # Конфигурация
```

---

## 7. ОПИСАНИЕ СТРАНИЦ

### 7.1 Главная (/)
**Контроллер:** HomeController.Index()  
**Данные:**
- Топ-5 спортсменов (по кол-ву достижений, затем по 1-м местам)
- Последние 5 новостей
- 2 featured-новости
- Первые 6 видов спорта (из команд)
- 3 ближайших соревнования
- Статистика: всего спортсменов, достижений, команд
- CTA-баннер "Стань частью спортивной истории"

### 7.2 Спортсмены (/Athlete)
**Контроллер:** AthleteController.Index()  
**Параметры:** searchString, sportType, sortOrder, page (пагинация 12/стр)  
**Вывод:** Сетка карточек с фото, именем, видом спорта, группой, кол-вом достижений  
**Фильтры:** Поиск по имени, фильтр по виду спорта, сортировка

### 7.3 Карточка спортсмена (/Athlete/Details/{id})
**Вывод:** Фото, имя, группа, команда, биография, рост/вес, статистика (всего/1/2/3 места), таймлайн достижений

### 7.4 Команды (/Team)
**Вывод:** Сетка карточек команд с названием, видом спорта, кол-вом спортсменов, тренером, описанием

### 7.5 Рейтинг (/Home/Leaderboard)
**Вывод:** Табличный список спортсменов с номером, фото, ФИО, группой, видом спорта, кол-вом золотых/серебряных/бронзовых/всех медалей. Фильтр по виду спорта.

### 7.6 Соревнования (/Competition)
**Вывод:** Две секции — "Предстоящие" (счётчик дней до даты) и "Прошедшие" (отмечает isCompleted). Карточки с названием, местом, датой, видом спорта.

### 7.7 Достижения (/Achievement)
**Вывод:** Список всех достижений с фильтрами по виду спорта и спортсмену.

### 7.8 Новости (/News)
**Вывод:** Сетка новостей с изображением, заголовком, категорией, датой, кратким содержанием. Детальная страница с полным текстом.

### 7.9 Поиск (/Home/Search)
**Параметр:** q (строка запроса)  
**Вывод:** Результаты по 4 категориям: спортсмены, команды, достижения, новости (ограничено 5 на категорию)

### 7.10 Контакты (/Home/Contact)
**Вывод:** Адрес, email, телефон, часы работы + форма обратной связи (POST на SendContact)

### 7.11 Админ-панель (/Admin)
**Вывод:** 
- Статистика (7 счётчиков: спортсмены, достижения, команды, колледжи, новости, соревнования, золотые медали)
- Два графика Chart.js: достижения по видам спорта (doughnut) и динамика по месяцам (bar)
- Карточки-ссылки на CRUD каждой сущности

### 7.12 Обработка ошибок
- 404: `/Error/404` — кастомная страница с иконкой и ссылкой на главную
- 500: `/Error/500` — сообщение об ошибке сервера
- `/Error` — общая страница

---

## 8. ЛОГИКА СИДОВ (SeedData)

При первом запуске (если таблица Athletes пуста) создаются:
- **2 колледжа** (Полоцк, Витебск)
- **6 команд** (баскетбол, волейбол, футбол, плавание, лёгкая атлетика, теннис)
- **12 спортсменов** (с именами, группами, ростом/весом, фото-ссылками)
- **20 достижений** (разные виды спорта, с 1/2/3 местами)
- **10 новостей** (разные категории, с featured)
- **5 соревнований** (3 предстоящих + 1 прошедшее + 1 завершённое)

Фото спортсменов в сидах указывают на `/images/athletes/...` (локальные файлы, которые нужно положить вручную).

---

## 9. ЗАГРУЗКА ФАЙЛОВ

- Фото через админку → сохраняется в `wwwroot/uploads/{GUID}.{ext}`
- URL в БД: `/uploads/{GUID}.{ext}`
- Папка `wwwroot/uploads/` создаётся автоматически при загрузке
- При редактировании можно указать URL вручную (поля `photoUrl` в форме)

---

## 10. ОСОБЕННОСТИ РЕАЛИЗАЦИИ

1. **Пагинация** — на странице списка спортсменов (12 на страницу), параметры фильтрации сохраняются
2. **Валидация** — Data Annotations на моделях + jQuery Validation на стороне клиента
3. **Таймлайн достижений** — визуальная хронология с цветными маркерами (золото/серебро/бронза)
4. **Чарты Chart.js** — кольцевая диаграмма и столбчатый график на админ-панели
5. **Анимации AOS.js** — появление элементов при скролле
6. **Адаптивность** — Bootstrap 5, mobile-first, print styles
7. **Безопасность** — SHA256 пароля, AntiForgeryToken, валидация returnUrl против Open Redirect
8. **Session-based auth** — вход через сессию, кастомный атрибут `[Authorize]`

---

## 11. КОНФИГУРАЦИЯ (appsettings.json)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=CollegeSports.db"
  },
  "Admin": {
    "Login": "admin",
    "PasswordHash": "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk="
  }
}
```

**Пароль по умолчанию:** admin123  
**Путь к БД (реальный, из Program.cs):** `{ContentRootPath}/App_Data/CollegeSports.db`

---

## 12. СТРУКТУРА _Layout.cshtml

**Шапка:**
- Bootstrap 5.3, Bootstrap Icons, AOS.js, Chart.js (только в админке)
- Кастомный `:root` с CSS-переменными (--dark, --accent, --surface, --card-bg, --text-color и др.)
- Glass-эффект на навбаре (backdrop-filter: blur)

**Навбар (слева направо):**
Логотип → Главная → Спортсмены → Команды → Рейтинг → Соревнования → Достижения → Новости → О нас → 🔍 Поиск → [Войти | Админ (dropdown)]

**Футер:**
3 колонки: логотип/соцсети → разделы → контакты

**Скрипты (общие):**
Bootstrap JS, AOS.init(), scroll-to-top button, jQuery + Validation
