using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class SpawnScroll : Item
    {
        private Type _monsterType;

        [Constructable]
        public SpawnScroll(Type monsterType) : base(0x1F5B) // Use a scroll graphic ID
        {
            Name = $"Scroll of {monsterType.Name}";
            _monsterType = monsterType;
            Weight = 1.0;
        }

        public SpawnScroll(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !from.Alive)
                return;

            if (_monsterType == null || !typeof(BaseCreature).IsAssignableFrom(_monsterType))
                return;

            BaseCreature creature = (BaseCreature)Activator.CreateInstance(_monsterType);
            if (creature != null)
            {
                creature.Controlled = true;
                creature.ControlMaster = from;
                creature.IsBonded = true;
                creature.MoveToWorld(from.Location, from.Map);

                from.SendMessage($"A {creature.Name} has been summoned!");
                Delete(); // Deletes the scroll after use
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(_monsterType.FullName);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
            _monsterType = Type.GetType(reader.ReadString());
        }
    }
}
