using System;
using Server;
using Server.Items;
using Server.Mobiles;
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("corpse of a Witch Apprentice")]
    public class WitchApprentice : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(10.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public WitchApprentice()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            // Random names list
            string[] names = new string[]
            {
                "Selka", "Brinelle", "Tavira", "Ysolde", "Merith", 
                "Anwyn", "Kaela", "Iskra", "Velyss", "Orlaith",
                "Sylenne", "Maris", "Thalara", "Vespera", "Nysha"
            };

            Name = names[Utility.Random(names.Length)];
            Title = "the Witch Apprentice";

            Body = 0x191; // Female human body
            Hue = 1153; // Pale with a hint of magical corruption

            SpeechHue = 2124;

            // Outfit - Arcane, youthful, and slightly chaotic
            AddItem(new PlainDress() { Hue = 1358, Name = "Academy Initiate's Dress" });
            AddItem(new Cloak() { Hue = 1175, Name = "Tattered Student Cloak" });
            AddItem(new WizardsHat() { Hue = 1175, Name = "Crescentmarked Hat" });
            AddItem(new Sandals() { Hue = 1150 });

            // Weapon - WildStaff for a budding witch
            AddItem(new WildStaff() { Hue = 1175, Name = "Unstable Spellstaff" });

            // Stats
            SetStr(90, 100);
            SetDex(90, 110);
            SetInt(180, 220);

            SetHits(100, 150);
            SetDamage(8, 12);

            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.EvalInt, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Meditation, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 65.0, 75.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 30;

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
                    int phrase = Utility.Random(6);

                    switch (phrase)
                    {
                        case 0: this.Say("Headmistress Malidor, guide my hand!"); break;
                        case 1: this.Say("You dare challenge an apprentice of the Academy?"); break;
                        case 2: this.Say("My power *will* awaken... in your ruin!"); break;
                        case 3: this.Say("*chanting in a forgotten tongue*"); break;
                        case 4: this.Say("The void hungers for you!"); break;
                        case 5: this.Say("Mistress, grant me strength..."); break;
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
                    case 0: this.Say("You'll regret crossing paths with me!"); break;
                    case 1: this.Say("No! I cannot fail my studies!"); break;
                    case 2: this.Say("*arcane sparks crackle in panic*"); break;
                    case 3: this.Say("The spirits warned me this would happen..."); break;
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
                this.Say("A curse upon you!");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                this.Say("Your strength won't save you from my spells!");
            }
        }

        public override void OnDeath(Container c)
        {
            this.Say("The Academy... will avenge me...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(200, 350);

            if (Utility.RandomDouble() < 0.15)
                PackItem(new BlackPearl(Utility.RandomMinMax(5, 15))); // Reagent drop

            if (Utility.RandomDouble() < 0.05)
                PackItem(new SpellWeaversWand()); // Rare loot: unstable wand from her studies
        }

        public WitchApprentice(Serial serial) : base(serial) { }

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
