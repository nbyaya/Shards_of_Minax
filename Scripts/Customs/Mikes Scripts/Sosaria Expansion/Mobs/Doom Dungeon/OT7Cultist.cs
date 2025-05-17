using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of an OT 7 Cultist")]
    public class OT7Cultist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(6.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public OT7Cultist()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            // Random names list
            string[] names = new string[]
            {
                "Velkar", "Thalric", "Dramus", "Malvek", "Zerik",
                "Korval", "Fenros", "Bravik", "Jorik", "Selvus",
                "Valkar", "Karnith", "Vorlek", "Trassik", "Eldros"
            };

            Name = names[Utility.Random(names.Length)];
            Title = "the OT 7 Cultist";

            Body = 0x190;
            Hue = 2305; // Sickly-grey/green tone, deeper corruption

            SpeechHue = 1359;

            // Outfit – distinguished and ominous
            AddItem(new Robe() { Hue = 1150, Name = "Overseer’s Ritual Robe" });
            AddItem(new LeatherGloves() { Hue = 1175, Name = "Gloves of Discipline" });
            AddItem(new Cloak() { Hue = 1175, Name = "Mantle of the Seventh Circle" });
            AddItem(new Boots() { Hue = 1175 });

            // Weapon – staff of authority
            AddItem(new WildStaff() { Hue = 1175, Name = "Staff of Exploitation" });

            // Stats – serious threat
            SetStr(250, 300);
            SetDex(100, 120);
            SetInt(180, 210);

            SetHits(300, 360);
            SetDamage(12, 18);

            SetSkill(SkillName.MagicResist, 80.0, 95.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Swords, 60.0, 80.0);
            SetSkill(SkillName.Magery, 85.0, 100.0);
            SetSkill(SkillName.EvalInt, 85.0, 100.0);
            SetSkill(SkillName.Meditation, 60.0, 80.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 32;

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

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12))
                {
                    int phrase = Utility.Random(8);

                    switch (phrase)
                    {
                        case 0: Say("You will kneel before the Seventh Circle."); break;
                        case 1: Say("OT 8s and 9s are my tools... you are my prey."); break;
                        case 2: Say("Your resistance funds our ascension."); break;
                        case 3: Say("We do not bleed—we *invest* pain."); break;
                        case 4: Say("*laughs coldly* Your defiance is taxable."); break;
                        case 5: Say("Soon, you too will pay your tithe... with your soul."); break;
                        case 6: Say("OT 6 watches. They demand progress."); break;
                        case 7: Say("*chants darkly* Exploit. Expand. Expend."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 10) && Utility.RandomBool())
            {
                int phrase = Utility.Random(5);

                switch (phrase)
                {
                    case 0: Say("You'll pay interest on that blow."); break;
                    case 1: Say("I *own* your defiance."); break;
                    case 2: Say("Striking an overseer? Foolish."); break;
                    case 3: Say("*scoffs* OT 8s take such beatings for me."); break;
                    case 4: Say("Pain is my capital."); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3)
            {
                Say("Submit and tithe your strength!");
            }

            // Chance to summon an OT 9 as cannon fodder
            if (Utility.RandomDouble() < 0.15)
            {
                SummonOT9(defender);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                Say("*growls* OT 8! To me!");
                SummonOT8(attacker);
            }
        }

        public override void OnDeath(Container c)
        {
            Say("OT 6 will avenge me... you are already *in debt*.");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(350, 500);
			PackItem(new AscendOrStagnate());
            PackItem(new Bandage(Utility.RandomMinMax(4, 8)));

            if (Utility.RandomDouble() < 0.10)
                PackItem(new GoldBracelet() { Name = "Bracelet of the Seventh Circle" });

            if (Utility.RandomDouble() < 0.06)
                PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
        }

        private void SummonOT9(Mobile target)
        {
            if (this.Map != null && this.Location != Point3D.Zero)
            {
                OT9Cultist minion = new OT9Cultist();
                minion.MoveToWorld(this.Location, this.Map);
                minion.Combatant = target;
                Say("*summons a disposable OT 9 cultist*");
            }
        }

        private void SummonOT8(Mobile target)
        {
            if (this.Map != null && this.Location != Point3D.Zero)
            {
                OT8Cultist minion = new OT8Cultist();
                minion.MoveToWorld(this.Location, this.Map);
                minion.Combatant = target;
                Say("*commands an OT 8 cultist into battle*");
            }
        }

        public OT7Cultist(Serial serial) : base(serial) { }

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
