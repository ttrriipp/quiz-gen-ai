# AGENTS.md

## Project
**QuizGen AI** — AI-Based Quiz Generator and Examination System for WinForms.

This document guides AI agents contributing to the project through AI-driven development. Follow it as the source of truth for scope, architecture, workflow, coding priorities, and guardrails.

---

## 1. Product Goal
Build a desktop system for **teachers/admins** and **students** that:
- lets teachers generate multiple-choice quizzes using an AI API
- allows teachers to review, edit, publish, and analyze quizzes
- lets students take quizzes with timer/navigation/review flow
- computes results automatically
- provides AI study recommendations after submission
- uses 5–10 required package extensions in meaningful ways

The system must demonstrate:
- modern desktop UI
- database-backed workflows
- external API integration with API key
- reporting/analytics
- security and logging

---

## 2. Scope Lock
### Included
- login with role-based routing
- roles: `admin`, `teacher`, `student`
- quiz lifecycle: `draft`, `published`, `archived`
- multiple-choice questions only
- teacher quiz CRUD
- AI quiz generation
- manual question editing
- student exam taking with timer and review answers
- automatic scoring
- teacher/admin dashboard and reports
- student results and progress
- AI study recommendations
- notifications and logging

### Excluded for now
- essay questions
- file uploads for lessons/PDF parsing
- online multiplayer/proctoring
- full LMS integration
- advanced class/section assignment unless time allows

Do not expand scope unless explicitly requested.

---

## 3. Required Tech Stack
### Core packages
Use these packages unless implementation constraints force an equivalent from the approved list:
- **Guna.UI2.WinForms** — modern WinForms UI
- **ScottPlot.WinForms** — dashboards and reports
- **RestSharp** — HTTP API requests
- **Newtonsoft.Json** — JSON parsing and serialization
- **EntityFramework 6** — ORM and data access for .NET Framework
- **System.Data.SQLite.Core** + **System.Data.SQLite.EF6** — embedded local database provider and EF6 integration
- **BCrypt.Net-Next** — password hashing
- **Serilog** + **Serilog.Sinks.File** — application logging
- **Tulpep.NotificationWindow** — WinForms toast notifications

### Installed baseline
The current project package line is:
- `Guna.UI2.WinForms` `2.0.4.7`
- `RestSharp` `114.0.0`
- `ScottPlot` `5.1.58`
- `ScottPlot.WinForms` `5.1.58`
- `Newtonsoft.Json` `13.0.4`
- `EntityFramework` `6.5.1`
- `System.Data.SQLite.Core` `1.0.119.0`
- `System.Data.SQLite.EF6` `1.0.119.0`
- `BCrypt.Net-Next` `4.1.0`
- `Serilog` `4.3.1`
- `Serilog.Sinks.File` `7.0.0`
- `Tulpep.NotificationWindow` `1.1.38`

### Package note
- Reporting charts now use `ScottPlot.WinForms` in the teacher reports UI.
- Notifications use `Tulpep.NotificationWindow` in the WinForms UI flow. Keep toast usage visible for login, quiz lifecycle, exam, and AI feedback states.
- Legacy `LiveChartsCore*` packages may still exist in the repo, but new chart/report work should target ScottPlot unless explicitly redirected.
- Keep all `System.Data.SQLite*` packages on the same version line. For this project, use the classic `1.0.119.0` line with EF6.

### Language/runtime
- C#
- .NET Framework 4.8 WinForms
- SQLite local database

---

## 4. User Roles and UX
### Teacher/Admin experience
After login, show:
- dashboard with total students, total quizzes, average score, charts
- quick action: Generate Quiz
- sidebar/nav:
  - Dashboard
  - Quizzes
  - Reports
  - Logs (optional but recommended)

### Student experience
After login, show sidebar/nav:
- Quizzes
- Results

Keep teacher/admin and student layouts clearly separated.

---

## 5. Primary UI Screens
### Authentication
- Login screen
- Email + password
- role-based redirect after success

### Teacher/Admin
#### Dashboard
Show:
- total students
- total quizzes
- average score
- charts
- generate quiz quick action

#### Quizzes
Show:
- search bar
- optional filters: subject, status
- button: `New AI Quiz`
- quiz cards containing:
  - subject
  - status badge
  - title
  - topic
  - number of questions
  - difficulty
  - duration
  - actions: review/edit, publish/unpublish, delete

#### Quiz Editor
Show:
- question cards
- each question has text + choices
- actions: edit, delete
- add manual question button
- save draft / publish actions

#### Reports
Show:
- average score charts
- subject mastery charts
- hardest questions
- late submissions or recent submissions table
- filters if possible: date, subject, quiz

### Student
#### Quizzes
Show available quizzes as cards with:
- title
- subject
- difficulty
- duration
- start button
- availability/status

#### Exam Screen
Show:
- timer
- question number
- question text
- answer choices
- next/previous navigation
- question tracker with answered/unanswered states
- review answers option before submit

#### Result Summary
After submit, show:
- score
- correct/wrong count
- AI study recommendations
- button: View All Results

#### Results Tab
Show:
- average score
- quizzes taken
- best quiz score
- progress chart
- results table

---

## 6. Data Model Baseline
Use this schema as the working baseline.

### users
- `id`
- `email` (unique)
- `password_hash`
- `name`
- `created_at`
- `updated_at`

### user_roles
- `id`
- `user_id` -> users
- `role` (`teacher`, `student`, `admin`)
- unique (`user_id`, `role`)

### subjects
- `id`
- `name`
- `created_at`

### quizzes
- `id`
- `title`
- `subject_id` -> subjects
- `topic`
- `difficulty` (`Easy`, `Medium`, `Hard`)
- `duration_minutes`
- `status` (`draft`, `published`, `archived`)
- `created_by` -> users
- `created_at`
- `updated_at`
- `ai_generated` (bool)
- `available_from` (nullable)
- `available_until` (nullable)

### questions
- `id`
- `quiz_id` -> quizzes
- `text`
- `question_type` (`multiple_choice`)
- `explanation` (nullable)
- `order_index`
- `created_at`
- `updated_at`

### choices
- `id`
- `question_id` -> questions
- `text`
- `is_correct`
- `order_index`

### student_attempts
- `id`
- `quiz_id` -> quizzes
- `student_id` -> users
- `attempt_number`
- `started_at`
- `submitted_at` (nullable)
- `score_percentage` (nullable)
- `time_spent_seconds`

### student_answers
- `id`
- `attempt_id` -> student_attempts
- `question_id` -> questions
- `selected_choice_id` -> choices (nullable)
- `is_correct`
- `answered_at`

### optional: ai_generations
- `id`
- `quiz_id` -> quizzes
- `prompt`
- `raw_response_json`
- `provider`
- `model_name`
- `created_at`

### Data rules
- a published quiz must have at least 1 question
- each question must have choices
- each question must have exactly 1 correct choice
- only students can create attempts
- only teachers/admins can create/edit/publish quizzes

---

## 7. Architecture Guidance
Favor a clean layered structure.

### Suggested folders
- `Forms/`
- `Forms/Teacher/`
- `Forms/Student/`
- `Controls/`
- `Models/`
- `Data/`
- `Services/`
- `DTOs/`
- `Enums/`
- `Helpers/`
- `Repositories/` (optional)
- `ViewModels/` (optional)

### Keep responsibilities separate
- **Forms**: presentation and interaction only
- **Services**: business logic, API calls, scoring, analytics
- **Data/DbContext**: persistence and relationships
- **DTOs**: API request/response shapes
- **Helpers**: reusable utility code

Avoid putting all logic directly in form code-behind.

---

## 8. Service Boundaries
Create services with clear responsibilities.

### AuthService
- login
- verify password with BCrypt
- fetch roles
- redirect target resolution

### QuizService
- create quiz
- update quiz
- validate publish readiness
- publish/unpublish/archive
- list/filter/search quizzes

### QuestionService
- add manual question
- edit question
- delete question
- validate correct choice rules

### AiQuizService
- build prompt
- call external AI API using RestSharp
- parse JSON using Newtonsoft.Json
- return normalized DTOs
- persist AI generation log if used

### ExamService
- start attempt
- save/update selected answers
- compute score on submit
- mark `is_correct`
- compute time spent

### RecommendationService
- derive weak areas from results
- request AI study recommendations
- return recommendation text/cards

### ReportService
- total students
- total quizzes
- average score
- hardest questions
- subject mastery
- student progress metrics

### LoggingService / Serilog wrapper (optional)
- write structured logs for auth, quiz, exam, API, errors

---

## 9. AI Integration Rules
### AI usage points
1. **Quiz generation** for teachers
2. **Study recommendations** after student submission

### Guardrails
- AI must not auto-publish quizzes
- AI-generated questions must be reviewed/editable before publishing
- always handle invalid or malformed JSON
- show user-friendly error feedback if API fails
- log requests/responses carefully and avoid exposing API keys

### Expected quiz generation flow
1. teacher opens `New AI Quiz`
2. teacher inputs subject, topic, difficulty, question count
3. app sends AI request
4. app parses JSON response
5. app previews generated questions
6. teacher edits/reviews
7. app saves quiz as `draft`
8. teacher publishes manually

### Suggested response shape
```json
[
  {
    "questionText": "...",
    "choices": ["...", "...", "...", "..."],
    "correctAnswer": "...",
    "explanation": "..."
  }
]
```

Normalize API output into internal DTOs before saving to the database.

---

## 10. Development Order (Mandatory Sequence)
Agents should follow this order unless explicitly redirected.

1. finalize scope, enums, schema
2. set up project and install packages
3. configure SQLite + EF6 data access and database initialization
4. seed initial users/subjects
5. build login and role routing
6. build teacher/student shell layouts
7. build teacher quiz list and manual quiz editor
8. build AI quiz generation flow
9. build publish/status workflow
10. build teacher dashboard
11. build student quiz list
12. build exam-taking flow
13. build result calculation
14. build student results + AI recommendations
15. build teacher reports
16. add notifications, logs, validation, polish
17. prepare demo data and presentation flow

Do **manual workflow first, AI second, analytics third, polish last**.

---

## 11. UI/UX Standards
### General
- use card-based layout for stats, quizzes, and questions
- use clear spacing and consistent margins/padding
- use status badges for `draft`, `published`, `archived`
- prefer readable forms over dense layouts
- include loading states, empty states, and error states

### Notifications
Use toast notifications for:
- login success/failure
- quiz saved
- quiz generated
- quiz published/unpublished
- exam started
- time warning
- exam submitted
- AI recommendation failure/success

### Teacher UX rules
- never let AI generation skip manual review
- show question count and publish readiness
- allow manual question addition at all times in editor

### Student UX rules
- save selected answers during navigation when possible
- show answered/unanswered tracker
- support review before final submission
- show clear submit confirmation

---

## 12. Validation Rules
Agents should implement these checks.

### Authentication
- email required
- password required
- invalid credentials must not reveal which field failed

### Quiz creation/editing
- title required
- subject required
- duration > 0
- difficulty required
- question text required
- all choices required
- exactly one correct choice

### Publish validation
A quiz cannot be published if:
- it has no questions
- any question has fewer than 2 choices
- any question has no correct answer
- status transition is invalid

### Exam validation
- student cannot start unpublished quiz
- student cannot submit nonexistent attempt
- answer selections must match quiz questions
- disable multiple submit requests

---

## 13. Reporting Requirements
The reports experience should visibly demonstrate chart/report package usage.

### Teacher/Admin dashboard/reports
- total students
- total quizzes
- average score
- pass/fail overview
- subject mastery
- hardest questions
- recent or late submissions table

### Student results
- average score
- quizzes taken
- best score
- progress chart over time
- results history table

---

## 14. Logging Requirements
Log at least these events:
- login success
- login failure
- quiz created
- quiz updated
- quiz published/unpublished
- AI request start/end/failure
- exam started
- exam submitted
- scoring completed
- unhandled exceptions

Use structured logs where possible.

---

## 15. Security Rules
- hash passwords with BCrypt only; never store plain text
- do not hardcode API keys in source files
- keep API keys in configuration/environment variables
- restrict role-based actions in service and UI layers
- never trust UI-only restrictions

---

## 16. Coding Style Expectations
- prefer simple, readable C# over clever abstractions
- keep methods focused and short when possible
- use async for API/network operations
- centralize enums/constants
- avoid duplicate validation logic where possible
- add comments only where they add real clarity

### Naming
- forms: `TeacherDashboardForm`, `StudentResultsForm`
- services: `QuizService`, `AiQuizService`
- DTOs: `AiQuizRequestDto`, `AiQuizResponseDto`
- enums: `QuizStatus`, `QuizDifficulty`, `QuestionType`

---

## 17. Agent Tasking Protocol
When an AI agent works on a task, it should:
1. identify the target module/screen/service
2. preserve existing scope and naming conventions
3. avoid unrelated refactors
4. state assumptions clearly in code comments or notes if needed
5. update adjacent layers only when necessary
6. keep UI and business logic reasonably separated

### Good tasks for agents
- create EF6 models and DbContext relationships
- build login form and auth flow
- implement teacher quiz cards UI
- add AI quiz generation service and DTOs
- implement exam timer and answer tracking
- add reports queries and chart data adapters

### Bad tasks for agents
- redesigning the entire architecture without request
- changing database IDs/enums without migration plan
- mixing all logic into one form
- expanding beyond MCQ scope

---

## 18. Definition of Done
A feature is done when:
- UI is functional
- validation exists
- database changes are wired through
- role access is respected
- happy path and likely error path work
- logging is added where relevant
- notifications/feedback are visible where relevant

Examples:
- **AI quiz generation is done** only if teacher can generate, preview, edit, save draft, and receive success/error feedback.
- **Exam flow is done** only if student can start, navigate, review, submit, and see scored results.

---

## 19. Demo Readiness Checklist
Before presentation/demo, ensure:
- seeded admin/teacher/student accounts exist
- at least 3 subjects exist
- at least 3 quizzes exist
- at least 2 student attempts exist
- charts display real-looking data
- AI API path has a fallback or clear demo plan
- logs can be shown if asked

---

## 20. Priority Order if Time Becomes Limited
If delivery is at risk, prioritize:
1. login + roles
2. manual quiz CRUD
3. AI quiz generation
4. student exam flow
5. automatic scoring
6. dashboard/reports basics
7. AI study recommendations
8. logs/polish/admin extras

The minimum viable project must still prove package usage and core workflow.

---

## 21. Non-Negotiables
- keep the project as a **WinForms desktop app**
- keep **multiple-choice only** unless scope changes
- keep AI generation **review-first**, never auto-publish
- keep package usage visible and explainable
- optimize for a strong school project/demo, not overengineering
