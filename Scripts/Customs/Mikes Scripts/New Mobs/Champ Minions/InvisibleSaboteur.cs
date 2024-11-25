using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an invisible saboteur")]
    public class InvisibleSaboteur : BaseCreature
    {
        private TimeSpan m_InvisDelay = TimeSpan.FromSeconds(30.0); // time between invisibility
        private DateTime m_NextInvisTime;

        [Constructable]
        public InvisibleSaboteur() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 0;
            Body = 0x190;
            Name = "Invisible Saboteur";
            Title = "the Saboteur";
			Team = 1;

            Item hoodedShroud = new Item(0x2684);
            hoodedShroud.Hue = 0;
            hoodedShroud.Layer = Layer.OuterTorso;
            hoodedShroud.Movable = false;
            AddItem(hoodedShroud);

            SetStr(200, 300);
            SetDex(300, 400);
            SetInt(300, 400);

            SetHits(150, 250);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.EvalInt, 90.0, 100.0);
            SetSkill(SkillName.Magery, 95.0, 100.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 40;

            m_NextInvisTime = DateTime.Now + m_InvisDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextInvisTime)
            {
                this.Hidden = true;
                Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(Reveal));
                m_NextInvisTime = DateTime.Now + m_InvisDelay;
            }

            // Heal self invisibly
            if (Hits < HitsMax * 0.5 && Utility.RandomDouble() < 0.3) // 30% chance to heal
            {
                this.Say(true, "You can't see me, but I can still fight!");
                Heal(Utility.RandomMinMax(10, 20));
            }
            
        }

        private void Reveal()
        {
            this.Hidden = false;
        }

        public override void GenerateLoot()
        {
            PackGold(100, 200);
            AddLoot(LootPack.Rich);
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
        }

        public InvisibleSaboteur(Serial serial) : base(serial)
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
