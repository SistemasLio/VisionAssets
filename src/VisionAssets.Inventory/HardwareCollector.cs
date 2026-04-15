using System.Text.Json;
using Microsoft.Management.Infrastructure;

namespace VisionAssets.Inventory;

/// <summary>Coleta hardware via WMI/CIM (sem Win32_Product).</summary>
public sealed class HardwareCollector
{
    private const string Ns = @"root\cimv2";

    public Task<CollectedHardware[]> CollectAsync(CancellationToken cancellationToken = default) =>
        Task.Run(
            () =>
            {
                using var session = CimSession.Create(null);
                var rows = new List<CollectedHardware>();
                QueryProcessors(session, rows);
                QueryMemory(session, rows);
                QueryDisks(session, rows);
                QueryVideo(session, rows);
                QueryNetwork(session, rows);
                QueryBios(session, rows);
                QueryBaseBoard(session, rows);
                QueryLogicalDisks(session, rows);
                return rows.ToArray();
            },
            cancellationToken);

    private static void QueryProcessors(CimSession session, List<CollectedHardware> rows)
    {
        foreach (var i in session.QueryInstances(Ns, "WQL", "SELECT Name, Manufacturer, NumberOfCores, NumberOfLogicalProcessors, MaxClockSpeed FROM Win32_Processor", null))
        {
            using (i)
            {
                var details = JsonSerializer.Serialize(
                    new
                    {
                        cores = i.GetValue("NumberOfCores"),
                        logical = i.GetValue("NumberOfLogicalProcessors"),
                        maxMHz = i.GetValue("MaxClockSpeed"),
                    });
                rows.Add(
                    new CollectedHardware(
                        "CPU",
                        i.GetString("Manufacturer"),
                        i.GetString("Name"),
                        null,
                        details));
            }
        }
    }

    private static void QueryMemory(CimSession session, List<CollectedHardware> rows)
    {
        foreach (var i in session.QueryInstances(Ns, "WQL", "SELECT Manufacturer, PartNumber, Capacity, Speed FROM Win32_PhysicalMemory", null))
        {
            using (i)
            {
                var capacity = i.GetValue("Capacity") is ulong b ? b : (object?)null;
                var details = JsonSerializer.Serialize(new { bytes = capacity, speedMhz = i.GetValue("Speed") });
                rows.Add(
                    new CollectedHardware(
                        "RAM",
                        i.GetString("Manufacturer"),
                        i.GetString("PartNumber"),
                        null,
                        details));
            }
        }
    }

    private static void QueryDisks(CimSession session, List<CollectedHardware> rows)
    {
        foreach (var i in session.QueryInstances(Ns, "WQL", "SELECT Model, SerialNumber, Size, InterfaceType, MediaType FROM Win32_DiskDrive", null))
        {
            using (i)
            {
                var details = JsonSerializer.Serialize(
                    new
                    {
                        size = i.GetValue("Size"),
                        iface = i.GetString("InterfaceType"),
                        media = i.GetString("MediaType"),
                    });
                rows.Add(
                    new CollectedHardware(
                        "DISK",
                        null,
                        i.GetString("Model"),
                        SanitizeSerial(i.GetString("SerialNumber")),
                        details));
            }
        }
    }

    private static void QueryVideo(CimSession session, List<CollectedHardware> rows)
    {
        foreach (var i in session.QueryInstances(
                     Ns,
                     "WQL",
                     "SELECT Name, AdapterRAM, DriverVersion, VideoProcessor FROM Win32_VideoController",
                     null))
        {
            using (i)
            {
                var details = JsonSerializer.Serialize(
                    new { adapterRam = i.GetValue("AdapterRAM"), driver = i.GetString("DriverVersion"), processor = i.GetString("VideoProcessor") });
                rows.Add(new CollectedHardware("GPU", null, i.GetString("Name"), null, details));
            }
        }
    }

    private static void QueryNetwork(CimSession session, List<CollectedHardware> rows)
    {
        foreach (var i in session.QueryInstances(
                     Ns,
                     "WQL",
                     "SELECT Name, MACAddress, NetConnectionID, PhysicalAdapter FROM Win32_NetworkAdapter WHERE PhysicalAdapter = TRUE AND MACAddress IS NOT NULL",
                     null))
        {
            using (i)
            {
                var details = JsonSerializer.Serialize(new { netConnection = i.GetString("NetConnectionID") });
                rows.Add(
                    new CollectedHardware(
                        "NET",
                        null,
                        i.GetString("Name"),
                        i.GetString("MACAddress"),
                        details));
            }
        }
    }

    private static void QueryBios(CimSession session, List<CollectedHardware> rows)
    {
        foreach (var i in session.QueryInstances(Ns, "WQL", "SELECT Manufacturer, SerialNumber, Version, ReleaseDate FROM Win32_BIOS", null))
        {
            using (i)
            {
                var details = JsonSerializer.Serialize(new { version = i.GetString("Version"), release = i.GetString("ReleaseDate") });
                rows.Add(
                    new CollectedHardware(
                        "BIOS",
                        i.GetString("Manufacturer"),
                        i.GetString("Version"),
                        SanitizeSerial(i.GetString("SerialNumber")),
                        details));
            }
        }
    }

    private static void QueryBaseBoard(CimSession session, List<CollectedHardware> rows)
    {
        foreach (var i in session.QueryInstances(Ns, "WQL", "SELECT Manufacturer, Product, SerialNumber FROM Win32_BaseBoard", null))
        {
            using (i)
            {
                rows.Add(
                    new CollectedHardware(
                        "MOTHERBOARD",
                        i.GetString("Manufacturer"),
                        i.GetString("Product"),
                        SanitizeSerial(i.GetString("SerialNumber")),
                        null));
            }
        }
    }

    private static void QueryLogicalDisks(CimSession session, List<CollectedHardware> rows)
    {
        foreach (var i in session.QueryInstances(
                     Ns,
                     "WQL",
                     "SELECT DeviceID, VolumeName, Size, FreeSpace, FileSystem, DriveType FROM Win32_LogicalDisk WHERE DriveType = 3",
                     null))
        {
            using (i)
            {
                var details = JsonSerializer.Serialize(
                    new
                    {
                        volume = i.GetString("VolumeName"),
                        fs = i.GetString("FileSystem"),
                        size = i.GetValue("Size"),
                        free = i.GetValue("FreeSpace"),
                    });
                rows.Add(new CollectedHardware("LOGICAL_DISK", null, i.GetString("DeviceID"), null, details));
            }
        }
    }

    private static string? SanitizeSerial(string? s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return null;
        var t = s.Trim();
        return t.Length == 0 || t.Equals("To be filled by O.E.M.", StringComparison.OrdinalIgnoreCase) ? null : t;
    }
}
