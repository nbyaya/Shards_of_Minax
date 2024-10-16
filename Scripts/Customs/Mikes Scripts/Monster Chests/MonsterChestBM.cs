using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Items
{
    public class MonsterChestBM : WoodenChest
    {
        private bool m_Activated = false;

        [Constructable]
        public MonsterChestBM() : base()
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

            int ancientLichCount = Utility.Random(1, 4); // Spawns 1 to 2 Ancient Liches
            int greaterDragonCount = Utility.Random(1, 4); // Spawns 1 to 2 Greater Dragons
            int balronCount = Utility.Random(1, 4); // Spawns 1 to 2 Balrons

            for (int i = 0; i < ancientLichCount; i++)
                monsters.Add(new AncientLich());

            for (int i = 0; i < greaterDragonCount; i++)
                monsters.Add(new GreaterDragon());

            for (int i = 0; i < balronCount; i++)
                monsters.Add(new Balron());

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

        public MonsterChestBM(Serial serial) : base(serial)
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
