using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a sawmill worker")]
    public class SawmillWorker : BaseCreature
    {
        private TimeSpan m_AttackDelay = TimeSpan.FromSeconds(5.0); // time between saw blade attacks
        public DateTime m_NextAttackTime;

        [Constructable]
        public SawmillWorker() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Sawmill Worker";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Sawmill Worker";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item apron = new HalfApron();
            Item boots = new Boots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(apron);
            AddItem(boots);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(700, 900);
            SetDex(150, 250);
            SetInt(100, 150);

            SetHits(500, 800);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 70);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.Anatomy, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 85.1, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);
            SetSkill(SkillName.Lumberjacking, 100.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 50;

            m_NextAttackTime = DateTime.Now + m_AttackDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextAttackTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    ShootSawBlade(combatant);
                    m_NextAttackTime = DateTime.Now + m_AttackDelay;
                }
            }
        }

        private void ShootSawBlade(Mobile target)
        {
            this.Say(true, "Take this, you filthy log!");

            // Create a saw blade item to shoot
            Item sawBlade = new SawBlade();
            sawBlade.MoveToWorld(target.Location, target.Map);

            // Deal damage to the target
            AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 100, 0, 0, 0, 0);
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 200);
            AddLoot(LootPack.Average);

            PackItem(new Board(Utility.RandomMinMax(10, 20)));
        }

        public SawmillWorker(Serial serial) : base(serial)
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

    public class SawBlade : Item
    {
        [Constructable]
        public SawBlade() : base(0x1BD4) // Graphic ID for a saw blade
        {
            Movable = false;
            Hue = 0; // Set color if desired
        }

        public SawBlade(Serial serial) : base(serial)
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
