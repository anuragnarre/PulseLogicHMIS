# GitHub MCP Server — Setup & Configuration Guide

This guide provides full instructions for setting up and configuring the **GitHub Model Context Protocol (MCP) Server** to allow AI assistants (such as Antigravity IDE, Cursor, or Claude Desktop) to interact directly with the [PulseLogicHMIS GitHub Repository](https://github.com/anuragnarre/PulseLogicHMIS).

---

## 🔑 Step 1: Create a GitHub Personal Access Token (PAT)

1. Go to **GitHub Settings** -> **Developer Settings** -> **Personal Access Tokens**.
2. Select **Tokens (classic)** or **Fine-grained tokens**.
   * For **Classic Token**: Select scopes `repo` (Full control of private repositories), `read:org`, `user`.
   * For **Fine-grained Token**: Select `anuragnarre/PulseLogicHMIS` repository and grant read/write access to **Contents**, **Pull Requests**, **Issues**, and **Metadata**.
3. Generate the token and copy the generated token string (starts with `ghp_` or `github_pat_`).

---

## ⚙️ Step 2: Configure MCP Server in Your Environment

### Option A: VS Code / Antigravity IDE Configuration

Add the following configuration to your MCP settings file (e.g., `.vscode/mcp.json` or global `mcp.json`):

```json
{
  "mcpServers": {
    "github": {
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-github"
      ],
      "env": {
        "GITHUB_PERSONAL_ACCESS_TOKEN": "YOUR_GITHUB_PAT_HERE"
      }
    }
  }
}
```

### Option B: Cursor IDE Configuration

1. Open **Cursor Settings** (`Ctrl + ,` or `Cmd + ,`).
2. Navigate to **Features** -> **MCP**.
3. Click **Add New MCP Server**.
4. Configure as follows:
   * **Name**: `github`
   * **Type**: `command`
   * **Command**: `npx -y @modelcontextprotocol/server-github`
   * **Environment Variable**: `GITHUB_PERSONAL_ACCESS_TOKEN=YOUR_GITHUB_PAT_HERE`

### Option C: Claude Desktop Configuration

Edit `%APPDATA%\Claude\claude_desktop_config.json` (Windows) or `~/Library/Application Support/Claude/claude_desktop_config.json` (macOS):

```json
{
  "mcpServers": {
    "github": {
      "command": "npx",
      "args": [
        "-y",
        "@modelcontextprotocol/server-github"
      ],
      "env": {
        "GITHUB_PERSONAL_ACCESS_TOKEN": "YOUR_GITHUB_PAT_HERE"
      }
    }
  }
}
```

---

## 🛠️ Step 3: Available MCP Capabilities

Once connected, the AI agent can perform the following GitHub operations autonomously:

| Capability | Description |
| :--- | :--- |
| **Search Code & Repos** | Query files, commits, and code across `PulseLogicHMIS` |
| **Manage Pull Requests** | Create, view, merge, and review PRs |
| **Manage Issues** | Create, comment on, and close GitHub issues |
| **Branch Management** | Create, list, or delete repository branches |
| **Commit Details** | View commit history, diffs, and blamelines |

---

## 🔒 Security Best Practices

> [!CAUTION]
> Never commit your raw Personal Access Token (`GITHUB_PERSONAL_ACCESS_TOKEN`) into source control or public repositories. Store it in local configuration files that are excluded via `.gitignore`.
