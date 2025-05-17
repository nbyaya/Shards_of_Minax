using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of an OT 8 Cultist")]
    public class OT8Cultist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(7.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public OT8Cultist()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Random names list
            string[] names = new string[]
            {
                "Sark", "Veylor", "Drask", "Mellik", "Ravos",
                "Tarn", "Elrik", "Bravik", "Fenlor", "Valk",
                "Ornus", "Kelric", "Javos", "Selvik", "Zarn"
            };

            Name = names[Utility.Random(names.Length)];
            Title = "the OT 8 Cultist";

            Body = 0x190;
            Hue = 2405; // Slightly darker, corrupted skin tone

            SpeechHue = 1150;

            // Outfit – fancier than OT 9, but still worn
            AddItem(new MonkRobe() { Hue = 1150, Name = "Supervisor’s Robe of the OT" });
            AddItem(new LeatherGloves() { Hue = 1175 });
            AddItem(new Sandals() { Hue = 1150 });

            // Weapon – blood ritual blade
            AddItem(new Kryss() { Hue = 1175, Name = "Blood-Tithe Kris" });

            // Stats – stronger than OT 9
            SetStr(200, 230);
            SetDex(100, 120);
            SetInt(120, 140);

            SetHits(180, 220);
            SetDamage(10, 16);

            SetSkill(SkillName.MagicResist, 65.0, 85.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Swords, 60.0, 80.0);
            SetSkill(SkillName.Magery, 70.0, 90.0);
            SetSkill(SkillName.EvalInt, 60.0, 80.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 22;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    int phrase = Utility.Random(7);

                    switch (phrase)
                    {
                        case 0: Say("You dare defy OT wisdom?"); break;
                        case 1: Say("OT 9s beg at my feet... you’ll grovel soon enough."); break;
                        case 2: Say("*sneers* The tithe grows ever deeper."); break;
                        case 3: Say("You will *fund* my ascension."); break;
                        case 4: Say("OT 7 watches... always."); break;
                        case 5: Say("*chants* Suffer. Pay. Obey."); break;
                        case 6: Say("Your resistance only delays my promotion."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 8) && Utility.RandomBool())
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: Say("I’ll invoice your pain to the OT treasury."); break;
                    case 1: Say("Bleed. It’s a tax write-off."); break;
                    case 2: Say("Striking an OT 8 is punishable by death."); break;
                    case 3: Say("Higher ranks will hear of this transgression."); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25)
            {
                Say("Your flesh pays my dues!");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                Say("*grits teeth* OT 7 will punish me if I fall...");
            }
        }

        public override void OnDeath(Container c)
        {
            Say("No! I was next in line for OT 7!");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(200, 350); // Slightly better than OT 9
			PackItem(new UnlockingTheBrokenStar());
            PackItem(new Bandage(Utility.RandomMinMax(2, 5)));

            if (Utility.RandomDouble() < 0.08)
                PackItem(new GoldRing() { Name = "Supervisor’s Ring of OT 8" }); // Rare drop

            if (Utility.RandomDouble() < 0.04)
                PackItem(new BlackPearl(Utility.RandomMinMax(2, 6))); // Minor reagent drop
        }

        public OT8Cultist(Serial serial) : base(serial) { }

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
