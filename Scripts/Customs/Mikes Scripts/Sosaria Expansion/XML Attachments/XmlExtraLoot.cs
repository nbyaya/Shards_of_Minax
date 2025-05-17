using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.XmlSpawner2
{
    public class XmlExtraLoot : XmlAttachment
    {
        private string _itemName;
        private int _min;
        private int _max;
        private double _chance;

        [CommandProperty(AccessLevel.GameMaster)]
        public string ItemName { get => _itemName; set => _itemName = value; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MinAmount { get => _min; set => _min = Math.Max(0, value); }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxAmount { get => _max; set => _max = Math.Max(_min, value); }

        [CommandProperty(AccessLevel.GameMaster)]
        public double Chance { get => _chance; set => _chance = Math.Max(0.0, Math.Min(1.0, value)); }

        public XmlExtraLoot(ASerial serial) : base(serial) { }

        [Attachable]
        public XmlExtraLoot(string itemName, int min, int max, double chance = 1.0)
        {
            _itemName = itemName;
            _min = Math.Max(0, min);
            _max = Math.Max(_min, max);
            _chance = Math.Max(0.0, Math.Min(1.0, chance));
        }

        public override void OnAttach()
        {
            base.OnAttach();

            if (!(AttachedTo is Mobile m)) return;
            if (Utility.RandomDouble() > _chance) return;

            var t = ScriptCompiler.FindTypeByName(_itemName);

            if (t == null || !typeof(Item).IsAssignableFrom(t))
                return;

            int amount = Utility.RandomMinMax(_min, _max);
            if (amount <= 0) return;

            try
            {
                var probe = (Item)Activator.CreateInstance(t);

                if (probe.Stackable)
                {
                    probe.Amount = amount;
                    m.AddToBackpack(probe);
                }
                else
                {
                    probe.Delete();
                    for (int i = 0; i < amount; i++)
                        m.AddToBackpack((Item)Activator.CreateInstance(t));
                }
            }
            catch
            {
                // Silent fail
            }

            // Self-delete after adding loot
            this.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(_itemName ?? "");
            writer.Write(_min);
            writer.Write(_max);
            writer.Write(_chance);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            _itemName = reader.ReadString();
            _min = reader.ReadInt();
            _max = reader.ReadInt();
            _chance = reader.ReadDouble();
        }

        public override string OnIdentify(Mobile from)
        {
            return $"Extra loot: {_itemName}, {_min}-{_max}x, {_chance:P0} chance.";
        }
    }
}
