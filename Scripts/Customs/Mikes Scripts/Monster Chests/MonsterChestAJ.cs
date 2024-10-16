using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class MonsterChestAJ : WoodenChest
    {
        private bool m_Activated = false;

        [Constructable]
        public MonsterChestAJ() : base()
        {
            this.Movable = true;
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            if (!m_Activated)
            {
                SpawnPirateOfTheStars(from);
                Timer.DelayCall(TimeSpan.FromSeconds(30), DeleteChest);
                m_Activated = true;
            }
        }

        private void SpawnPirateOfTheStars(Mobile from)
        {
            PirateOfTheStars pirate = new PirateOfTheStars();

            pirate.MoveToWorld(from.Location, from.Map);
            pirate.Combatant = from;

            Effects.SendLocationEffect(Location, Map, 0x36CB, 50, 10); // Explosion effect
        }

        private void DeleteChest()
        {
            this.Delete();
        }

        public MonsterChestAJ(Serial serial) : base(serial)
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
