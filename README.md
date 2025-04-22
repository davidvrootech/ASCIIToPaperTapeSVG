# ASCII2PaperTape (C# Edition)

## Overview

This C# application is a port of the original **8-Bit ASCII to Paper Tape SVG Generator** by [colemanjw2](https://github.com/colemanjw2). It converts ASCII text, including control codes, into an SVG representation of a 1-inch-wide 8-bit paper tape. The output is suitable for lasercutting or for retro computing enthusiasts building paper tape programs for vintage machines like the Altair 8800.

This version maintains the spirit and functionality of the original Python tool while offering a Windows Forms GUI built in C#. Users can generate, preview, and save paper tape SVGs with customizable text wrapping options.

---

## 🎉 Features

- **8-Bit ASCII Encoding** – Supports standard ASCII characters and extended control codes.
- **Control Code Parsing** – Accepts `<0xNN>` syntax for control characters (e.g., `<0x07>` for BEL).
- **SVG Output** – Generates a clean, scalable SVG file representing punched holes in paper tape.
- **Custom Line Widths** – Toggle between 40-column, 80-column, or unlimited width formatting.
- **Retro Computing Friendly** – Includes paper tape markers for STX, ETX, BEL, etc.
- **GUI-Based** – Built with Windows Forms for an intuitive graphical user experience.

---
![image](https://github.com/user-attachments/assets/fff93063-cc04-48a3-9256-fc12c6dba3dc)


## 🖥️ Installation & Usage

### Prerequisites

- Windows with .NET Framework or .NET Core installed (depending on build target)
- No external packages required beyond standard .NET libraries

### Run the Program

1. Clone this repository:
   ```bash
   git clone https://github.com/YOUR_USERNAME/ASCII2PaperTape-CSharp.git
   cd ASCII2PaperTape-CSharp
Open the project in Visual Studio or run the compiled .exe.

Enter your ASCII text and control codes using the <0xNN> format:

swift
Copy
Edit
<0x02>HELLO, WORLD!<0x0A>Press any key to continue...<0x07><0x03>
Choose a text wrapping mode:

40 Columns

80 Columns

Unlimited

Click Generate SVG and save the file.

🧾 Example Input
Input:

swift
Copy
Edit
<0x02>HELLO, WORLD!<0x0A>Press any key to continue...<0x07><0x03>
Explanation:

STX (<0x02>) – Start of text

BEL (<0x07>) – Bell (makes a sound)

ETX (<0x03>) – End of text

Regular characters like "HELLO, WORLD!" are punched in standard ASCII

🔍 Output Preview
SVG files visually represent paper tape punch patterns

Each tape segment is labeled for organization and continuous feed alignment

Compatible with lasercutting and retro computing displays

📚 ASCII Reference
Refer to ascii-code.com for a full list of control characters and their meanings.

Enhancement Ideas:

Include alternate tape formats or widths

🙏 Acknowledgments
Original Python Project by colemanjw2: ASCII2PaperTape

ASCII table reference: ascii-code.com

Inspired by the golden age of retro computing and paper tape programming.

👋 Contributions Welcome!
If you have ideas for improvements, feel free to fork this repo and submit a pull request. Let's keep the spirit of vintage computing alive — one punched hole at a time!
