# 🚗 Car Insurance Assistant Bot

A Telegram bot that automates the car insurance workflow using AI-powered document parsing and policy generation. The bot extracts information from passport and vehicle documents, confirms data with the user, calculates insurance cost, and generates a dummy insurance policy using OpenAI.

---

## 📦 Setup Instructions

### ✅ Prerequisites

- .NET 8+ SDK
- Docker (optional for deployment)
- Telegram bot token from @BotFather
- OpenAI API key from OpenAI

### 🛠 Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/your-username/CarBot.git
   cd CarBot

2. **Add environment variables:**
Create a .env file in the root directory:

    ```bash
    TELEGRAM_TOKEN=your_telegram_bot_token
    OPENAI_API_KEY=your_openai_api_key
    MINDEE_API_TOKEN=your_mindee_api_key

3. **Install dependencies:**

   ```bash
   dotnet restore

4. **Run the bot locally:**

   ```bash
   dotnet run

### 🔁 Bot Workflow
-User starts the bot with /start or /create.

- Bot requests a photo of the passport.

- Data is extracted (Full Name, Birth Date, ID Number) and confirmed with the user.

- Bot requests a photo of the vehicle document.

- Data is extracted (Brand, Capacity, Color, Weight, VIN) and confirmed with the user.

- Bot presents an insurance cost.

- Upon acceptance, a dummy policy PDF is generated using OpenAI and sent to the user.

### 💬 Example Interaction
👤 User: /start

🤖 Bot: Hello! I'm your Car Insurance Assistant Bot.
You can send me a photo of your car documents or ask questions about car insurance.
        
🤖 Bot: Please submit a photo of your passport and then your vehicle identification document.

👤 User sends photo of passport

🤖 Bot: Extracting data from image...
       
🤖 Bot: Extracted Data:
Last Name: Doe
First Name: John
Country: GBR
ID Number: 123456789
Birth Date: 1992-04-21
Please confirm if this is correct. (Yes / No)

👤 User: Yes

🤖 Bot: Passport confirmed. Now send the vehicle identification document.

👤 User sends vehicle photo

🤖 Bot: Extracting data from image...
Brand: Toyota
Capacity: 1800cc
Color: Black
Weight: 1500kg
VIN: XYZ123ABC456
Please confirm if this is correct. (Yes / No)

👤 User: Yes

🤖 Bot: The cost of your insurance is $100. Do you accept? (Yes / No)

👤 User: Yes

🤖 Bot: Insurance confirmed. Cost: $100. Thank you!
Generating your car insurance document...

🤖 Bot sends PDF: `InsurancePolicy.pdf`

🤖 Bot: Thank you. Feel free to ask any questions!

### 🚀 Deployment
This bot can be deployed using:

🟪 Railway.app (background worker, free trial up to 30 days)
🟧 Render.com (not availible free trial for background worker)

### Docker
To build and run with Docker:
  ```bash
  docker build -t car-bot .
  docker run --env-file .env car-bot

