using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of an OT 9 Cultist")]
    public class OT9Cultist : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(8.0);
        public DateTime m_NextSpeechTime;

        [Constructable]
        public OT9Cultist()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            // Random names list
            string[] names = new string[]
            {
                "Ravik", "Pell", "Dron", "Vallis", "Kern",
                "Obrek", "Jallus", "Fenrik", "Var", "Mol", 
                "Kerris", "Dask", "Brin", "Lok", "Sarn"
            };

            // Assign random name and fixed title
            Name = names[Utility.Random(names.Length)];
            Title = "the OT 9 Cultist";

            Body = 0x190;
            Hue = 2220; // sickly pale

            SpeechHue = 1175;

            // Outfit – ragged but with grim cult motifs
            AddItem(new DeathRobe() { Hue = 1175, Name = "Threadbare Initiate’s Robe" });
            AddItem(new Sandals() { Hue = 1175 });
            AddItem(new HoodedShroudOfShadows() { Hue = 1175, Name = "OT Cowl of Poverty" });

            // Weapon - desperate, makeshift
            AddItem(new ButcherKnife() { Hue = 1109, Name = "Blood-Tithe Knife" });

            // Stats - weakish melee mob
            SetStr(150, 180);
            SetDex(90, 110);
            SetInt(40, 60);

            SetHits(120, 160);
            SetDamage(6, 12);

            SetSkill(SkillName.MagicResist, 40.0, 60.0);
            SetSkill(SkillName.Tactics, 50.0, 70.0);
            SetSkill(SkillName.Swords, 50.0, 70.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 16;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        // Speech behavior
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
                        case 0: Say("Pay your dues or pay in blood!"); break;
                        case 1: Say("OT 8s take my gold... I’ll take your life!"); break;
                        case 2: Say("No refunds. No mercy."); break;
                        case 3: Say("*mutters* Can't believe I'm still paying monthly fees..."); break;
                        case 4: Say("Your soul might cover my overdue tithe."); break;
                        case 5: Say("OT 10s are expendable. Just like you."); break;
                        case 6: Say("*nervously counts coins during combat*"); break;
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
                    case 0: Say("You'll regret that. The higher ranks *never* pay for healing."); break;
                    case 1: Say("This wound's going to cost me."); break;
                    case 2: Say("*groans* OT 7s make me pay for my own bandages."); break;
                    case 3: Say("Your strike has been noted... and invoiced."); break;
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
                Say("I will *climb the ranks* with your blood!");
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.2)
            {
                Say("OT 8 will punish me for this...");
            }
        }

        public override void OnDeath(Container c)
        {
            Say("No... I was *this close* to becoming OT 8...");
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            PackGold(100, 200); // They're broke cultists, mostly
			PackItem(new SevenStepsToAscension());
            PackItem(new Bandage(Utility.RandomMinMax(1, 4)));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GoldRing() { Name = "Tithe Ring of OT 9" }); // Rare drop, symbol of their rank
        }

        public OT9Cultist(Serial serial) : base(serial) { }

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
