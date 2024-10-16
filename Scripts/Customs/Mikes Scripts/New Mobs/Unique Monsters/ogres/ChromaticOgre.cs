using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using System.Threading;

namespace Server.Mobiles
{
    public class ChromaticOgre : BaseCreature
    {
        private static readonly int MaxHue = 3000;
        private static readonly int MinHue = 1;
        private static readonly int CycleDelay = 2000; // Delay in milliseconds (2 seconds)

        private Timer _hueTimer;

        [Constructable]
        public ChromaticOgre() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Hue #1 Ogre";
            Body = 1; // Ogre body
            BaseSoundID = 0x62;
            Hue = MinHue; // Start with the first hue
            UpdateNameAndHue(); // Update the name and hue on creation
			
            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            _hueTimer = new HueTimer(this);
            _hueTimer.Start();
        }

        private void UpdateNameAndHue()
        {
            Name = $"Hue #{Hue} Ogre";
        }

        public override void OnDelete()
        {
            base.OnDelete();
            if (_hueTimer != null)
            {
                _hueTimer.Stop();
                _hueTimer = null;
            }
        }

        private class HueTimer : Timer
        {
            private readonly ChromaticOgre _ogre;

            public HueTimer(ChromaticOgre ogre) : base(TimeSpan.Zero, TimeSpan.FromMilliseconds(CycleDelay))
            {
                _ogre = ogre;
            }

            protected override void OnTick()
            {
                if (_ogre == null || _ogre.Deleted)
                    return;

                int newHue = (_ogre.Hue + 1 > MaxHue) ? MinHue : _ogre.Hue + 1;
                _ogre.Hue = newHue;
                _ogre.UpdateNameAndHue();
            }
        }

        public ChromaticOgre(Serial serial) : base(serial)
        {
        }
		
        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
