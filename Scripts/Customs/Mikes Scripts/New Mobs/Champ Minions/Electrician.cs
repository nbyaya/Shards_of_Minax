using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an electrician")]
    public class Electrician : BaseCreature
    {
        private TimeSpan m_TrapDelay = TimeSpan.FromSeconds(15.0); // time between trap setups
        public DateTime m_NextTrapTime;

        [Constructable]
        public Electrician() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 1;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Electrician";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Electrician";
            }

            if (Utility.RandomBool())
            {
                Item suit = new Robe();
                suit.Hue = 1154; // Blue color
                AddItem(suit);
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item shoes = new Shoes(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            Item tool = new GnarledStaff();
            AddItem(hair);
            AddItem(shoes);
            AddItem(tool);
            tool.Movable = false;

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(500, 800);
            SetDex(150, 250);
            SetInt(200, 300);

            SetHits(400, 600);

            SetDamage(8, 16);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Energy, 75);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 40, 55);
            SetResistance(ResistanceType.Poison, 60, 75);
            SetResistance(ResistanceType.Energy, 70, 85);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;

            m_NextTrapTime = DateTime.Now + m_TrapDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextTrapTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Feel the power of electricity!"); break;
                        case 1: this.Say(true, "Let's see how you handle this shock!"); break;
                        case 2: this.Say(true, "Get ready to be fried!"); break;
                        case 3: this.Say(true, "You'll be electrified!"); break;
                    }

                    // Setup an electric trap
                    SetupElectricTrap();

                    m_NextTrapTime = DateTime.Now + m_TrapDelay;
                }

                base.OnThink();
            }
        }

        private void SetupElectricTrap()
        {
            // Logic for setting up an electric trap
            // Placeholder: You can replace it with the actual trap creation logic
            this.Say(true, "*sets up an electric trap*");
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "The current... it's too strong..."); break;
                case 1: this.Say(true, "You've short-circuited me..."); break;
            }

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }

        public override int Damage(int amount, Mobile from)
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
            {
                if (Utility.RandomBool())
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "You think that'll stop me?!"); break;
                        case 1: this.Say(true, "Is that all you got?"); break;
                        case 2: this.Say(true, "I've taken on worse than you!"); break;
                        case 3: this.Say(true, "You're in over your head!"); break;
                    }
                }
            }

            return base.Damage(amount, from);
        }

        public Electrician(Serial serial) : base(serial)
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
