using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a Senior Year Witch")]
    public class SeniorYearWitch : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(12.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public SeniorYearWitch()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            // Random names list
            string[] names = new string[]
            {
                "Selira", "Vessara", "Kaelen", "Orintha", "Mirael",
                "Ysara", "Thalira", "Brenna", "Liorae", "Calith",
                "Nyssara", "Elvyn", "Maedra", "Faylen", "Sirae"
            };

            Name = names[Utility.Random(names.Length)];
            Title = "the Senior Year Witch";

            Body = 0x191; // Female human
            Hue = 1150; // Pale magical hue
            SpeechHue = 1153;

            // Outfit - scholarly witch with practical flair
            AddItem(new FancyShirt() { Hue = 1109, Name = "Malidor Academy Tunic" });
            AddItem(new Skirt() { Hue = 1154, Name = "Runebound Skirt" });
            AddItem(new WizardsHat() { Hue = 1153, Name = "Senior Year Hat of Focus" });
            AddItem(new Boots() { Hue = 1150 });

            // Weapon - a wand imbued with unstable magic
            AddItem(new SpellWeaversWand() { Hue = 1161, Name = "Experimental Arcane Wand" });

            // Light armor for duels and quick movement
            AddItem(new LeatherGloves() { Hue = 1157 });

            // Stats
            SetStr(80, 100);
            SetDex(90, 110);
            SetInt(220, 250);

            SetHits(250, 300);
            SetDamage(8, 14);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Meditation, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 6000;
            Karma = -2000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } } // Some may choose to engage, others may not
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ClickTitle { get { return true; } }

        // Idle / combat speech logic
        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12))
                {
                    int phrase = Utility.Random(7);

                    switch (phrase)
                    {
                        case 0: this.Say("You think a final-year witch would fall so easily?"); break;
                        case 1: this.Say("By Malidor’s sigil, I shall *endure*!"); break;
                        case 2: this.Say("The academy didn't prepare me for *you*... but I'll adapt."); break;
                        case 3: this.Say("*Murmurs an unstable incantation under her breath*"); break;
                        case 4: this.Say("Do you always interrupt scholarly work with violence?"); break;
                        case 5: this.Say("My thesis? Survival."); break;
                        case 6: this.Say("*Eyes glow with arcane fury*"); break;
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
                    case 0: this.Say("You'll regret striking a scholar of Malidor."); break;
                    case 1: this.Say("Bruises will fade. Knowledge remains."); break;
                    case 2: this.Say("*Utters a warding glyph that crackles with blue fire*"); break;
                    case 3: this.Say("You'll feed my research notes with your mistakes."); break;
                    case 4: this.Say("Every wound teaches me more."); break;
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
                this.Say("Lesson one: never drop your guard!");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                this.Say("Noted. I'll recalibrate my defense.");
            }
        }

        public override void OnDeath(Container c)
        {
            this.Say("This... will be on the final exam...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(350, 600);

            if (Utility.RandomDouble() < 0.3)
                PackItem(new MagicWand()); // Loot representing her field of study

            if (Utility.RandomDouble() < 0.15)
                PackItem(new Robe() { Hue = 1153, Name = "Tattered Malidor Robe" }); // Unique clothing drop
        }

        public SeniorYearWitch(Serial serial) : base(serial) { }

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
