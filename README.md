# 🦅 hacker78010 cr3w - Advanced Intelligent Port Scanner Suite (Pro Edition)

---

## 📊 Project Metadata & Vital Signs

| Registry Property | Specification Details |
| :--- | :--- |
| **Core Architecture** | Fully Autonomous Multi-Threaded Task System (.NET Framework) |
| **Development Group** | **hacker78010 cr3w** |
| **Engine Compilation** | C# Coded |
| **Deployment Model** | 100% Standalone Portable Executable Binary (`.exe`) |
| **Release Stability** | v1.0.0 Stable Production Grade |
| **Target Landscapes** | Local Area Networks (LAN), Localhosts, Wide Area Networks (WAN/Public IPs) |

---

## ⚡ Executive Summary & Project Overview

The **hacker78010 cr3w Port Scanner** is a high-performance, lightweight, and thread-safe networking utility designed to conduct rapid asynchronous security audits across the entire TCP port spectrum (`1 - 65535`). 

Unlike traditional, legacy scanning utilities that sequentially attempt connections—resulting in UI lockups and massive time delays—this suite leverages an advanced **Asynchronous Task Architecture (`Task.WhenAll`)** combined with low-level WinSock bindings. It is specifically engineered to dynamically balance throughput, allowing users to probe public infrastructure without instantly triggering automated Firewall drop policies or DDoS mitigation rate-limiters.

---

## 🚀 Advanced Core Architectural Features

### 1. High-Speed Asynchronous Task Multi-Threading
The scanning engine breaks down the monolithic `65535` port matrix into controlled, high-density connection batches. By utilizing modern asynchronous primitives, the tool maintains an active pool of up to hundreds of simultaneous socket requests, preventing the execution pipeline from bottlenecking on closed or filtered endpoints.

### 2. Intelligent Network Throttling & WAN Adaptability
Standard port scanners fail on Public Internet Routing due to packet loss and strict network security configurations. This suite utilizes an **Adaptive Latency Matrix** (configured at a precise `450ms - 500ms` connection timeout boundary), giving data packets ample time to traverse public hops while maintaining an incredibly aggressive overall execution speed.

### 3. Official IANA Protocol Service Resolver
When an open port is intercepted, the engine filters the discovery through an embedded translation matrix based on official Internet Assigned Numbers Authority (IANA) registries. This translates raw integers into human-readable corporate and enterprise services on the fly.

### 4. Thread-Safe Emergency Interruption Pipeline
By implementing a centralized `CancellationTokenSource` pattern, the application gives the operator absolute control over the network footprint. Pressing the **STOP** button immediately drops all active socket connection handles and releases allocated system memory without crashing or freezing the Graphical User Interface.

### 5. 100% Zero-Dependency Portable Footprint
The final compilation bypasses external runtime dependencies, dynamic link libraries (`.dll`), or supplementary C++ engines. The entire tool lives inside a single, clean executable file that can be deployed instantly from any storage medium.

---

## 🎨 Clean Light Theme UI Specifications

The graphical interface is built utilizing a high-contrast **White Theme Design**, optimized for long diagnostic sessions and pristine legibility:

* **Prismatic Control Array:** Segregated control fields with professional Windows-Blue (`#0078D7`) action inputs and Warning-Red (`#E81123`) interruption states.
* **Granular Progress Matrix:** Real-time progress bar synchronized with the background thread pooling system, giving a visual 1:1 map of the execution phase.
* **Verbose Diagnostic Console:** High-fidelity scrolling output console utilizes specialized system fonts (`Consolas`) to maintain absolute column alignment for all output flags.

---

## 🔍 Exhaustive Service & Port Matrix Mapping Index

The internal dictionary maps discovered open states to their official networking classifications:

### Core System & Well-Known Ports (1 - 1023)
| Port | Protocol | Default Service | Core Operational Description |
| :--- | :--- | :--- | :--- |
| **20** | TCP | `FTP-Data` | File Transfer Protocol (Data Channel) |
| **21** | TCP | `FTP-Control` | File Transfer Protocol (Command Channel) |
| **22** | TCP | `SSH` | Secure Cryptographic Shell & Remote Management |
| **23** | TCP | `Telnet` | Legacy Unencrypted Text Communications |
| **25** | TCP | `SMTP` | Simple Mail Transfer Protocol (Routing/Relay) |
| **53** | TCP | `DNS` | Domain Name System Infrastructure |
| **67 / 68** | UDP/TCP | `DHCP` | Dynamic Host Configuration Protocol |
| **80** | TCP | `HTTP` | Unencrypted Hypertext Web Services |
| **110** | TCP | `POP3` | Post Office Protocol version 3 (Email Retrieval) |
| **123** | UDP/TCP | `NTP` | Network Time Protocol (Clock Sync) |
| **135** | TCP | `RPC` | Microsoft Remote Procedure Call Endpoint Mapper |
| **139** | TCP | `NetBIOS` | Legacy Windows Session Sharing Services |
| **143** | TCP | `IMAP` | Internet Message Access Protocol (Mail Management) |
| **389** | TCP/UDP | `LDAP` | Lightweight Directory Access Protocol |
| **443** | TCP | `HTTPS` | TLS/SSL Encrypted Secure Web Infrastructure |
| **445** | TCP | `SMB` | Microsoft Directory Services (File System Share) |
| **514** | UDP/TCP | `Syslog` | Centralized System Logging Port |
| **587** | TCP | `SMTP-Secure` | Authenticated Secure Email Submission |
| **993** | TCP | `IMAPS` | SSL Encrypted IMAP Email Fetching |
| **995** | TCP | `POP3S` | SSL Encrypted POP3 Email Fetching |

### Registered Enterprise & Database Ports (1024 - 49151)
| Port | Protocol | Default Service | Core Operational Description |
| :--- | :--- | :--- | :--- |
| **1433** | TCP | `MSSQL` | Microsoft SQL Server Relational Database Management |
| **1521** | TCP | `Oracle` | Oracle Database listener infrastructure |
| **2049** | TCP/UDP | `NFS` | Network File System Protocol |
| **3306** | TCP | `MySQL` | Open-Source Relational Database Engine |
| **3389** | TCP | `RDP` | Microsoft Remote Desktop Protocol Utility |
| **5432** | TCP | `PostgreSQL` | Advanced Object-Relational Database Server |
| **5900** | TCP | `VNC` | Virtual Network Computing Remote Access |
| **8080** | TCP | `HTTP-Alt` | Common Alternative Web Server or Proxy Node |
| **8443** | TCP | `HTTPS-Alt` | Common Alternative Secure Web Server |

### Dynamic & Private Ranges (49152 - 65535)
* Ports residing within this band are dynamically categorized by the engine as **Dynamic / Private / Ephemeral Allocation Blocks**. These channels are safely verified and outputted to the logging panel with dynamic structural labels.

---

## 🛠️ Code Architecture Snippet

The following segment highlights the asynchronous connection mapping logic used inside the scanning pipeline:

```csharp
private async Task ScanPortAsync(IPAddress ip, int port, CancellationToken token)
{
    if (token.IsCancellationRequested) return;

    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
    {
        try
        {
            var connectTask = socket.ConnectAsync(new IPEndPoint(ip, port));
            var delayTask = Task.Delay(500, token); 

            var completedTask = await Task.WhenAny(connectTask, delayTask);

            if (completedTask == connectTask && socket.Connected && !token.IsCancellationRequested)
            {
                string serviceName = GetPortService(port);
                // Valid connection registered successfully
            }
        }
        catch { /* Suppress dead socket drop exceptions */ }
    }
}

