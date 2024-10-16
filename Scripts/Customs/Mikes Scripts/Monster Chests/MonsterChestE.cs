using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class MonsterChestE : WoodenChest
    {
        private bool m_Activated = false;

        [Constructable]
        public MonsterChestE() : base()
        {
				this.Movable = true;
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

            int ogreCount = Utility.Random(1, 4);      // Adjust numbers as desired
            int orcbruteCount = Utility.Random(1, 5);  // Adjust numbers as desired
            int trollCount = Utility.Random(1, 4);     // Adjust numbers as desired

            for (int i = 0; i < ogreCount; i++)
                monsters.Add(new Ogre());

            for (int i = 0; i < orcbruteCount; i++)
                monsters.Add(new OrcBrute());

            for (int i = 0; i < trollCount; i++)
                monsters.Add(new Troll());

            foreach (BaseCreature monster in monsters)
            {
                monster.MoveToWorld(from.Location, from.Map);
                monster.Combatant = from;
            }

            Effects.SendLocationEffect(Location, Map, 0x36CB, 50, 10); // Explosion effect
        }

        private void DeleteChest()
        {
            this.Delete();
        }

        public MonsterChestE(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
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
