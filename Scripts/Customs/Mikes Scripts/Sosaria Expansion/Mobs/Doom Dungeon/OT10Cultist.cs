using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a Doom Cultist")]
    public class OT10Cultist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(12.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public OT10Cultist()
            : base(AIType.AI_Mage, FightMode.Closest, 8, 1, 0.2, 0.4)
        {
            // Random names list
            string[] names = new string[]
            {
                "Veyl", "Orlen", "Kazir", "Dauth", "Selrik",
                "Morrek", "Harl", "Vasik", "Keloth", "Brivan",
                "Yarrek", "Drosil", "Fenroth", "Halvik", "Marn"
            };

            Name = names[Utility.Random(names.Length)];
            Title = "the OT 10 Cultist";

            Body = 0x190;
            Hue = 1109; // Pale, sickly grey

            SpeechHue = 1153;

            // Outfit — ragged, eerie, low-status cult gear
            AddItem(new DeathRobe() { Hue = 1150, Name = "Tattered Cultist Robe" });
            AddItem(new HoodedShroudOfShadows() { Hue = 1150, Name = "Shroud of the Forgotten" });
            AddItem(new Sandals() { Hue = 1108 });

            // Weapon — weak staff, symbolic
            AddItem(new GnarledStaff() { Hue = 1102, Name = "Warped Staff of the Low Circle" });

            // Stats — weak for combat, but dangerous if ignored
            SetStr(50, 70);
            SetDex(40, 60);
            SetInt(80, 100);

            SetHits(90, 120);
            SetDamage(5, 10);

            SetSkill(SkillName.Magery, 60.0, 80.0);
            SetSkill(SkillName.EvalInt, 60.0, 75.0);
            SetSkill(SkillName.MagicResist, 55.0, 70.0);
            SetSkill(SkillName.Tactics, 45.0, 60.0);

            Fame = 1000;
            Karma = -3000;

            VirtualArmor = 12;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer => true;
        public override bool ClickTitle => true;
        public override bool CanRummageCorpses => false;

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    int phrase = Utility.Random(8);

                    switch (phrase)
                    {
                        case 0: this.Say("The Void sees me... it HUNGERS."); break;
                        case 1: this.Say("I am but a pawn... and yet you fear me!"); break;
                        case 2: this.Say("Pain... is the only teacher we have left."); break;
                        case 3: this.Say("*mutters an unsettling chant under breath*"); break;
                        case 4: this.Say("The higher circles offer us to the shadows..."); break;
                        case 5: this.Say("I will not break before you. Not yet."); break;
                        case 6: this.Say("The Broken Star will rise again."); break;
                        case 7: this.Say("*eyes glaze over, trembling* The voices are... louder now."); break;
                    }

                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }
            }
        }

        public override int Damage(int amount, Mobile from)
        {
            if (from != null && from.Map == this.Map && from.InRange(this, 8) && Utility.RandomDouble() < 0.33)
            {
                int phrase = Utility.Random(4);

                switch (phrase)
                {
                    case 0: this.Say("Your strikes only feed the darkness."); break;
                    case 1: this.Say("I bleed... and the Void smiles."); break;
                    case 2: this.Say("*gasps* I was never meant to survive this..."); break;
                    case 3: this.Say("The higher cultists sent me to *die*!"); break;
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
                this.Say("I strike in the name of the Broken Star!");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                this.Say("*coughs blood* My pain will echo in the Void...");
            }
        }

        public override void OnDeath(Container c)
        {
            this.Say("They promised... I would be spared...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(50, 100);
			PackItem(new TheStarBeyondScarcity());

            if (Utility.RandomDouble() < 0.1)
                PackItem(new BoneHarvester() { Hue = 1109, Name = "Warped Blade Shard" });

            if (Utility.RandomDouble() < 0.05)
                PackItem(new Bloodmoss()); // symbolic for low-circle magic
        }

        public OT10Cultist(Serial serial) : base(serial) { }

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
