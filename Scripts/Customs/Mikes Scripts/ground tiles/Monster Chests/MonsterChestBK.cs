using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class MonsterChestBK : WoodenChest
    {
        private bool m_Activated = false;

        [Constructable]
        public MonsterChestBK() : base()
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

            // Spawn a RockabillyRevenant
            RockabillyRevenant revenant = new RockabillyRevenant();
            revenant.MoveToWorld(from.Location, from.Map);
            revenant.Combatant = from;

            Effects.SendLocationEffect(Location, Map, 0x36CB, 50, 10); // Explosion effect
        }

        private void DeleteChest()
        {
            this.Delete();
        }

        public MonsterChestBK(Serial serial) : base(serial)
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
