using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a Knight of Justice")]
    public class KnightOfJustice : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0); // time between Knight's speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public KnightOfJustice() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190; // male body
            Name = NameList.RandomName("male");
            Title = "the Knight of Justice";
			Team = 1;

            // Equipment
            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item armor = new PlateChest();
            Item pants = new PlateLegs();
            Item boots = new PlateArms();
            Item weapon = new Longsword();

            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(armor);
            AddItem(pants);
            AddItem(boots);
            AddItem(weapon);

            weapon.Movable = false;

            SetStr(900, 1300);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(800, 1200);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 50, 65);
            SetResistance(ResistanceType.Cold, 50, 65);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 55);

            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Swords, 100.0, 120.0);
            SetSkill(SkillName.Parry, 90.0, 110.0);
            SetSkill(SkillName.Chivalry, 80.0, 100.0);

            Fame = 10000;
            Karma = 10000;

            VirtualArmor = 70;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return true; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(4);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "Justice will prevail!"); break;
                        case 1: this.Say(true, "For the weak and oppressed!"); break;
                        case 2: this.Say(true, "You shall not harm the innocent!"); break;
                        case 3: this.Say(true, "Feel the wrath of justice!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(300, 400);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "Justice is served..."); break;
                case 1: this.Say(true, "My duty is done..."); break;
            }

            PackItem(new Bandage(Utility.RandomMinMax(10, 20)));
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
                        case 0: this.Say(true, "You cannot defeat justice!"); break;
                        case 1: this.Say(true, "Your attacks are futile!"); break;
                        case 2: this.Say(true, "I fight for the weak!"); break;
                        case 3: this.Say(true, "Justice is unyielding!"); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }

            return base.Damage(amount, from);
        }

        public KnightOfJustice(Serial serial) : base(serial)
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
