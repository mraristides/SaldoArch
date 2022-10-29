using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Saldo.SaldoAPI.Entities;

public class Transacao
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }

    [BsonElement("userid")]
    public int userid { get; set; }

    [BsonElement("date")]
    public DateTime date { get; set; }

    [BsonElement("value")]
    public double value { get; set; }
}
