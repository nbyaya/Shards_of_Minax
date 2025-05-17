using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a ghost scout")]
    public class GhostScout : BaseCreature
    {
        private TimeSpan m_SpeechDelay = TimeSpan.FromSeconds(15.0); // time between ghost scout speech
        public DateTime m_NextSpeechTime;

        [Constructable]
        public GhostScout() : base(AIType.AI_Thief, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x3CA; // Ghost body
            Hue = 0x4001; // Transparent hue
            Name = "Ghost Scout";
            Team = 2;

            SetStr( 150, 200 );
            SetDex( 250, 300 );
            SetInt( 150, 200 );

            SetHits( 200, 300 );

            SetDamage( 5, 10 );

            SetDamageType( ResistanceType.Physical, 50 );
            SetDamageType( ResistanceType.Cold, 50 );

            SetResistance( ResistanceType.Physical, 40, 50 );
            SetResistance( ResistanceType.Fire, 20, 30 );
            SetResistance( ResistanceType.Cold, 60, 70 );
            SetResistance( ResistanceType.Poison, 30, 40 );
            SetResistance( ResistanceType.Energy, 50, 60 );

            SetSkill( SkillName.Hiding, 100.0 );
            SetSkill( SkillName.Stealth, 100.0 );
            SetSkill( SkillName.Anatomy, 50.0, 70.0 );
            SetSkill( SkillName.Tactics, 60.0, 80.0 );
            SetSkill( SkillName.Wrestling, 70.0, 90.0 );

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSpeechTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    int phrase = Utility.Random(3);

                    switch (phrase)
                    {
                        case 0: this.Say(true, "I see all..."); break;
                        case 1: this.Say(true, "You can't hide from me."); break;
                        case 2: this.Say(true, "Your position is known."); break;
                    }
                    
                    m_NextSpeechTime = DateTime.Now + m_SpeechDelay;
                }

                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(100, 150);
            AddLoot(LootPack.Rich);

            int phrase = Utility.Random(2);

            switch (phrase)
            {
                case 0: this.Say(true, "My mission... is complete..."); break;
                case 1: this.Say(true, "You have... bested me..."); break;
            }

        }

        public GhostScout(Serial serial) : base(serial)
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
