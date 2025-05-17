using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a second year witch")]
    public class SecondYearWitch : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public SecondYearWitch()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Random name list
            string[] names = new string[]
            {
                "Ilyra", "Branneth", "Mirael", "Sylaine", "Kessira",
                "Thalen", "Virella", "Orinai", "Jassara", "Maelith",
                "Elowen", "Nerisse", "Tiranel", "Viondra", "Cassith"
            };

            Name = names[Utility.Random(names.Length)];
            Title = "the Second Year Witch";

            Body = 0x191; // Female human
            Hue = 1153; // Slightly mystical shade

            SpeechHue = 2075;

            // Outfit — youthful arcane style
            AddItem(new PlainDress() { Hue = 1175, Name = "Academy Uniform Robe" });
            AddItem(new WizardsHat() { Hue = 1175, Name = "Initiate’s Hat" });
            AddItem(new Sandals() { Hue = 1175 });

            // Weapon - Beginner's Mystic Staff
            AddItem(new WildStaff() { Hue = 1175, Name = "Unstable Apprentice Staff" });

            // Stats
            SetStr(80, 100);
            SetDex(60, 80);
            SetInt(160, 200);

            SetHits(150, 200);
            SetDamage(8, 14);

            SetSkill(SkillName.EvalInt, 90.0, 100.0);
            SetSkill(SkillName.Magery, 95.0, 110.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 60.0, 75.0);
            SetSkill(SkillName.Wrestling, 40.0, 50.0);

            Fame = 4000;
            Karma = -2000; // Not evil, just a bit reckless

            VirtualArmor = 28;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        // Dynamic combat speech — OnThink()
        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    int phrase = Utility.Random(6);

                    switch (phrase)
                    {
                        case 0: this.Say("This is *totally* what Professor Malidor taught us. I think."); break;
                        case 1: this.Say("By the Three Moons... let this spell *work*!"); break;
                        case 2: this.Say("*frantically flips through spellbook*"); break;
                        case 3: this.Say("You’re making me miss alchemy class for this?!"); break;
                        case 4: this.Say("Don’t underestimate a Second Year! We *almost* passed summoning last semester!"); break;
                        case 5: this.Say("I knew I should’ve chosen healing magic..."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        // When damaged
        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 8) && Utility.RandomBool())
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say("Ouch! That was *rude*!"); break;
                    case 1: this.Say("Professor Kel’s shield spell would’ve blocked that..."); break;
                    case 2: this.Say("*panicked muttering* Not the face!"); break;
                    case 3: this.Say("That’s going to bruise. Badly."); break;
                }

                m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.2)
            {
                this.Say("Feel the wrath of... oh wait, wrong spell!");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                this.Say("Why does everyone *always* go for the mage?");
            }
        }

        public override void OnDeath(Container c)
        {
            this.Say("*coughs* At least... I don’t have to take the final exam...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(200, 300);

            if (Utility.RandomDouble() < 0.3)
                PackItem(new Spellbook() { Hue = 1175, Name = "Frayed Apprentice Spellbook" });

            if (Utility.RandomDouble() < 0.1)
                PackItem(new MandrakeRoot(5)); // Represents low-level reagents

            if (Utility.RandomDouble() < 0.05)
                PackItem(new MagicWand()); // Rare chance at a wand drop
        }

        public SecondYearWitch(Serial serial) : base(serial) { }

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
