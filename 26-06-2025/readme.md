
# 🐧 Linux Permissions & User Management


## 🔐 1. What happens when you run `chmod +x file.sh` on a file with `644` permissions?

#### Initial Permission:
-rw-r--r-- (644)

#### After Executing:
```bash
chmod +x file.sh
```
#### Result:

-rwxr-xr--  (755)

#### Explanation:
Execution (x) is granted to the owner, group, and others.

The file becomes an executable script, commonly required for .sh files.

## ✍️ 2. Compare chmod 744 file.txt with chmod u=rwx,go=r file.txt

#### Both commands result in the same permission set:

-rwxr--r--  (744)

| Notation     | Type     | Meaning                          |
| ------------ | -------- | -------------------------------- |
| `744`        | Octal    | Shorthand permission bits        |
| `u=rwx,go=r` | Symbolic | Descriptive permission breakdown |

#### Breakdown:

Owner: Read, write, execute

Group: Read-only

Others: Read-only

## 📌 3. What is the Sticky Bit?

#### Description:
The sticky bit restricts file deletion inside a directory.

#### Purpose:
Only the file owner (or root) can delete or rename their files—even if others have write access to the directory.

#### Example:
```
chmod +t /shared_folder
```

Common Usage:
/tmp directory

Team-shared folders

## ⚙️ 4. How to grant: Owner full access, group execute only, others no access?

#### Command:

```
chmod u=rwx,g=x,o= file.txt
```
#### Resulting Permissions:

-rwx--x---

## 📎 5. What is umask and why does it matter?

#### Definition:
umask (user file creation mask) defines which permission bits should not be set by default.

#### System Defaults:

Files: 666 → rw-rw-rw-

Directories: 777 → rwxrwxrwx

#### Example:
```
umask 022
```

#### Resulting Defaults:

Files → 644 → rw-r--r--

Dirs → 755 → rwxr-xr-x

#### Why Important:

Applies automatic permission filtering

Prevents over-permissive file creation

## 📊 6. What does umask 022 produce?


| Resource  | Base Default | umask | Final Permissions   |
| --------- | ------------ | ----- | ------------------- |
| File      | `666`        | `022` | `644` → `rw-r--r--` |
| Directory | `777`        | `022` | `755` → `rwxr-xr-x` |


## 🛡️ 7. Why use different umask values?

| umask | Environment         | Resulting Access         | Use Case                 |
| ----- | ------------------- | ------------------------ | ------------------------ |
| `002` | Development         | `rw-rw-r--`, `rwxrwxr-x` | Team-shared files        |
| `027` | Staging/Production  | `rw-r-----`, `rwxr-x---` | Group-limited access     |
| `077` | Secure/Confidential | `rw-------`, `rwx------` | Private user-only access |

## 👥 8. useradd vs adduser — What's the Difference?

| Feature       | `useradd`             | `adduser`                     |
| ------------- | --------------------- | ----------------------------- |
| Type          | Low-level binary      | High-level interactive script |
| Interactivity | No                    | Yes                           |
| Output        | Silent                | Prompts for shell, home, etc. |
| Usage         | Scripting, automation | Manual account setup          |

#### Examples:

```
# Create user with useradd
sudo useradd -m -s /bin/bash newuser

# Create user interactively with adduser
sudo adduser newuser
```