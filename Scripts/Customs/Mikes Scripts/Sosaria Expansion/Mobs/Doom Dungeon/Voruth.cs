using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of Voruth, the Eclipsed Tongue")]
    public class Voruth : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(3.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public Voruth()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "Voruth";
            Title = "the Eclipsed Tongue - OT2";

            Body = 0x190;
            Hue = 2309; // Skin marked with void sigils and a faint shifting aura

            SpeechHue = 1109;

            // Outfit – barely recognizably human, ceremonial and terrible
            AddItem(new Cloak() { Hue = 1109, Name = "Mantle of the Star’s Whisper" });
            AddItem(new MonkRobe() { Hue = 1109, Name = "Vestment of Unspoken Oaths" });
            AddItem(new LeatherGloves() { Hue = 1152, Name = "Fingers of Revelation" });
            AddItem(new Sandals() { Hue = 1109 });

            // No weapon. His voice *is the weapon.*

            // Stats – terrifying boss
            SetStr(400, 450);
            SetDex(160, 180);
            SetInt(420, 480);

            SetHits(900, 1100);
            SetDamage(25, 30);

            SetSkill(SkillName.MagicResist, 140.0, 150.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Magery, 145.0, 160.0);
            SetSkill(SkillName.EvalInt, 145.0, 160.0);
            SetSkill(SkillName.Meditation, 130.0, 140.0);

            Fame = 40000;
            Karma = -40000;

            VirtualArmor = 65;

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

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 14))
                {
                    int phrase = Utility.Random(7);

                    switch (phrase)
                    {
                        case 0: Say("*a voice not his own* The contract was sealed before your birth."); break;
                        case 1: Say("You labor beneath the false notion of choice."); break;
                        case 2: Say("All debts culminate in oblivion."); break;
                        case 3: Say("*words fracture the air itself* Ascension is a lie. Compliance is survival."); break;
                        case 4: Say("I have seen the balance sheet of the stars."); break;
                        case 5: Say("*whispers in forgotten tongues* The flesh remembers. The void collects."); break;
                        case 6: Say("Your breath is borrowed time. The interest is due."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 10) && Utility.RandomBool())
            {
                Say("*a voice beyond hearing murmurs calculations of suffering.*");
            }

            return base.Damage(amount, from);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3)
            {
                Say("Your flesh accrues error. Correction is inevitable.");
            }

            if (Utility.RandomDouble() < 0.25)
            {
                DeliverRevelation(defender);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                Say("Resistance... accelerates the final audit.");
            }
        }

        public override void OnDeath(Container c)
        {
            Say("*a ripple of void-sound escapes his form* The tongue is silenced... but the star persists...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(1800, 2200);

            if (Utility.RandomDouble() < 0.15)
                PackItem(new GoldBracelet() { Name = "Whisperband of Voruth" });

            if (Utility.RandomDouble() < 0.1)
                PackItem(new BlackPearl(Utility.RandomMinMax(25, 35)));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GoldRing() { Name = "Signet of the Eclipsed Tongue" });
        }

        private void DeliverRevelation(Mobile target)
        {
            if (target != null && target.Alive)
            {
                Say("*a dreadful syllable unfolds, warping the air*");

                target.FixedParticles(0x37CC, 10, 15, 5052, EffectLayer.Head);
                target.PlaySound(0x1F7);

                // Revelation effect: panic/fear (paralyze briefly)
                target.Paralyze(TimeSpan.FromSeconds(3));

                target.SendMessage(1109, "A terrible revelation roots you in place.");
            }
        }

        public Voruth(Serial serial) : base(serial) { }

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
