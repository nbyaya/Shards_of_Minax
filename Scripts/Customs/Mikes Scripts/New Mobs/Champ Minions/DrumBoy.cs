using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a DrumBoy")]
    public class DrumBoy : BaseCreature
    {
        private TimeSpan m_BoostInterval = TimeSpan.FromSeconds(10.0); // time between morale boosts
        public DateTime m_NextBoostTime;

        [Constructable]
        public DrumBoy() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the DrumBoy";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the DrumBoy";
            }

            Item drum = new Item(Utility.RandomList(0x0E9C, 0x0E9D)); // Randomly chooses between two drum graphics
            drum.Hue = Utility.RandomNeutralHue();
            drum.Movable = false;
            AddItem(drum);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(pants);
            AddItem(boots);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 700);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 40;

            m_NextBoostTime = DateTime.Now + m_BoostInterval;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextBoostTime)
            {
                BoostAllies();
                m_NextBoostTime = DateTime.Now + m_BoostInterval;
            }

            base.OnThink();
        }

        private void BoostAllies()
        {
            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m is BaseCreature)
                {
                    BaseCreature bc = (BaseCreature)m;
                    if (bc.Controlled && bc.ControlMaster == this.ControlMaster)
                    {
                        bc.PlaySound(0x55); // Sound of drums
                        bc.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                        bc.SendMessage("You feel invigorated by the DrumBoy's beat!");

                        bc.Dex += 10; // Increase attack speed by boosting dexterity
                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), delegate { bc.Dex -= 10; }); // Duration of the boost
                    }
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
            AddLoot(LootPack.Meager);
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
        }

        public DrumBoy(Serial serial) : base(serial)
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
