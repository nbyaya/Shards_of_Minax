using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a Malidor Professor")]
    public class MalidorProfessor : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(12.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public MalidorProfessor()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Random names list
            string[] names = new string[]
            {
                "Professor Vyrelle", "Professor Thessa", "Professor Caldrys", "Professor Alwen", "Professor Marnis",
                "Professor Veyra", "Professor Solvane", "Professor Trass", "Professor Lirian", "Professor Sorthys"
            };

            Name = names[Utility.Random(names.Length)];
            Title = "of Malidor Academy";

            Body = 0x191; // Female human body. Change to 0x190 if male versions are needed.
            Hue = 1150; // Dark bluish/arcane tone

            SpeechHue = 2125;

            // Outfit - academic and arcane
            AddItem(new Robe() { Hue = 1109, Name = "Robe of Arcane Instruction" });
            AddItem(new WizardsHat() { Hue = 1109, Name = "Hat of Fractured Wisdom" });
            AddItem(new Sandals() { Hue = 1150, Name = "Steps of the Voidbound" });
            AddItem(new BodySash() { Hue = 1175, Name = "Sash of Faculty Rank" });

            // Weapon - Arcane Staff
            AddItem(new WildStaff() { Hue = 1175, Name = "Lecturer’s Wild Staff" });

            // Stats
            SetStr(150, 200);
            SetDex(90, 120);
            SetInt(400, 500);

            SetHits(350, 400);
            SetDamage(10, 15);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.Tactics, 75.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 40;

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
                    int phrase = Utility.Random(6);

                    switch (phrase)
                    {
                        case 0: this.Say("You dare challenge the faculty? How quaint."); break;
                        case 1: this.Say("Another mind ripe for dissection..."); break;
                        case 2: this.Say("I wrote the texts *your teachers fear to read.*"); break;
                        case 3: this.Say("Ignorance is not a shield."); break;
                        case 4: this.Say("*Arcane energies swirl dangerously around her hands.*"); break;
                        case 5: this.Say("Your intrusion will be annotated... in blood."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 10) && Utility.RandomBool())
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say("You misunderstand pain. Let me clarify."); break;
                    case 1: this.Say("A foolish assault. Typical."); break;
                    case 2: this.Say("*snarls a binding hex*"); break;
                    case 3: this.Say("Your resistance amuses me."); break;
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
                this.Say("Consider this a... *practical demonstration.*");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.25)
            {
                this.Say("Your brutish methods betray you.");
            }
        }

        public override void OnDeath(Container c)
        {
            this.Say("Malidor's wisdom... will outlive me...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(500, 800);

            // Unique loot chance
            if (Utility.RandomDouble() < 0.25)
                PackItem(new SpellWeaversWand() { Name = "Fractured Wand of Malidor", Hue = 1175 });

            // Arcane scrolls or rare reagents
            if (Utility.RandomDouble() < 0.15)
                PackItem(new BlackPearl(Utility.RandomMinMax(10, 25)));

            if (Utility.RandomDouble() < 0.1)
                PackItem(new ArcaneGem()); // Rare drop
        }

        public MalidorProfessor(Serial serial) : base(serial) { }

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
