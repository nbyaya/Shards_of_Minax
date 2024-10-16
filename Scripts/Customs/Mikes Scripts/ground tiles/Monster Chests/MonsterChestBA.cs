using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class MonsterChestBA : WoodenChest
    {
        private bool m_Activated = false;

        [Constructable]
        public MonsterChestBA() : base()
        {
            Movable = true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            if (!m_Activated)
            {
                SpawnCreatures(from);
                Timer.DelayCall(TimeSpan.FromSeconds(30), DeleteChest);
                m_Activated = true;
            }
        }

        private void SpawnCreatures(Mobile from)
        {
            List<BaseCreature> monsters = new List<BaseCreature>();

            int skaSkaldCount = Utility.Random(1, 6);

            for (int i = 0; i < skaSkaldCount; i++)
                monsters.Add(new SkaSkald()); // Assuming SkaSkald is a BaseCreature derived class.

            foreach (BaseCreature monster in monsters)
            {
                monster.MoveToWorld(from.Location, from.Map);
                monster.Combatant = from;
            }

            Effects.SendLocationEffect(Location, Map, 0x36CB, 50, 10); // Explosion effect
        }

        private void DeleteChest()
        {
            Delete();
        }

        public MonsterChestBA(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
            writer.Write(m_Activated);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Activated = reader.ReadBool();
        }
    }
}
