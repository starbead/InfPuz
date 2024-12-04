using System.IO;


public class GameEvent
{
    public GameEventType eventType;
    private MemoryStream stream = null;
    private BinaryWriter writer = null;
    private BinaryReader reader = null;
    public bool useMemoryStream { get; private set; }

    public GameEvent() { }

    public GameEvent(GameEventType inEventType, bool isUseMemoryStream = false)
    {
        Init(inEventType, isUseMemoryStream);
    }

    protected void Init(GameEventType inEventType, bool isUseMemoryStream = false)
    {
        eventType = inEventType;
        useMemoryStream = isUseMemoryStream;
        if (useMemoryStream)
        {
            if (null == stream)
            {
                stream = new MemoryStream();
            }
            else
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(0);
            }

            if (null == writer)
                writer = new BinaryWriter(stream);
            if (null == reader)
                reader = new BinaryReader(stream);
        }
    }

    public void Close()
    {
        if (null != writer)
            writer.Close();
        writer = null;

        if (null != reader)
            reader.Close();
        reader = null;

        if (null != stream)
            stream.Close();
        stream = null;
    }

    public void Reset()
    {
        if (useMemoryStream)
            stream.Seek(0, SeekOrigin.Begin);
    }
   
    public void Write(long inData)
    {
        writer.Write(inData);
    }

    public void Write(int inData)
    {
        writer.Write(inData);
    }

    public void Write(double inData)
    {
        writer.Write(inData);
    }

    public void Write(float inData)
    {
        writer.Write(inData);
    }

    public void Write(bool inData)
    {
        writer.Write(inData);
    }

    public void Write(ulong inData)
    {
        writer.Write(inData);
    }

    public void Write(string inData)
    {
        writer.Write(inData);
    }

    public static implicit operator GameEventType(GameEvent _val)
    {
        return _val.eventType;
    }

    public double ReadDouble => reader.ReadDouble();
    public int ReadInt => reader.ReadInt32();
    public long ReadLong => reader.ReadInt64();
    public ulong ReadULong => reader.ReadUInt64();
    public float ReadFloat => reader.ReadSingle();
    public bool ReadBool => reader.ReadBoolean();
    public string ReadString => reader.ReadString();
}
