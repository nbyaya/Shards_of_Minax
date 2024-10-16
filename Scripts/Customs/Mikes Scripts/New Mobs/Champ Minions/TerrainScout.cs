using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a terrain scout")]
    public class TerrainScout : BaseCreature
    {
        private TimeSpan m_TerrainChangeDelay = TimeSpan.FromSeconds(20.0); // time between terrain changes
        public DateTime m_NextTerrainChangeTime;

        [Constructable]
        public TerrainScout() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Terrain Scout";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Terrain Scout";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomNeutralHue();
            AddItem(robe);

            Item sandals = new Sandals();
            sandals.Hue = Utility.RandomNeutralHue();
            AddItem(sandals);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(400, 500);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 60.0, 80.0);
            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.Meditation, 60.0, 80.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 50;

            m_NextTerrainChangeTime = DateTime.Now + m_TerrainChangeDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextTerrainChangeTime)
            {
                ChangeTerrain();
                m_NextTerrainChangeTime = DateTime.Now + m_TerrainChangeDelay;
            }
        }

        private void ChangeTerrain()
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                // Example terrain change effect - slowing down the combatant
                combatant.SendMessage("The terrain shifts beneath your feet, slowing you down!");
                combatant.Frozen = true;
                Timer.DelayCall(TimeSpan.FromSeconds(3.0), delegate { combatant.Frozen = false; });

                // Add additional terrain change logic here as needed
            }
        }

        public TerrainScout(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
