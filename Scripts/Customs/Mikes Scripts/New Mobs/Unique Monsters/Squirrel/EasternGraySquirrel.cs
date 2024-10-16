using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an eastern gray squirrel corpse")]
    public class EasternGraySquirrel : BaseCreature
    {
        private DateTime m_NextCamouflage;
        private DateTime m_NextNutToss;
        private DateTime m_NextNutShield;
        private DateTime m_NextSquirrelSwarm;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public EasternGraySquirrel()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "an Eastern Gray Squirrel";
            Body = 0x116; // Squirrel body
            Hue = 2437; // Gray hue

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

            m_AbilitiesInitialized = false;
        }

        public EasternGraySquirrel(Serial serial)
            : base(serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
                    m_NextNutToss = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
                    m_NextNutShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 40));
                    m_NextSquirrelSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextCamouflage)
                {
                    Camouflage();
                }

                if (DateTime.UtcNow >= m_NextNutToss)
                {
                    NutToss();
                }

                if (DateTime.UtcNow >= m_NextNutShield)
                {
                    NutShield();
                }

                if (DateTime.UtcNow >= m_NextSquirrelSwarm)
                {
                    SquirrelSwarm();
                }
            }
        }

        private void Camouflage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eastern Gray Squirrel blends into its surroundings! *");
            PlaySound(0x1F6); // Sound for blending

            // Make the squirrel semi-transparent and increase dodge chance
            this.Hue = 0; // Set hue to transparent
            this.Paralyzed = false;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
            {
                this.Hue = 1153; // Reset to original hue
            });

            m_NextCamouflage = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void NutToss()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eastern Gray Squirrel throws a nut! *");
            PlaySound(0x11B); // Nut toss sound

            Point3D loc = Location;
            NutItem nut = new NutItem();
            nut.MoveToWorld(loc, Map);

            Timer.DelayCall(TimeSpan.FromSeconds(2), () => ExplodeNut(nut));

            m_NextNutToss = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void ExplodeNut(NutItem nut)
        {
            if (nut.Deleted)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The nut explodes in a burst of confusion! *");
            PlaySound(0x307); // Explosion sound

            Effects.SendLocationEffect(nut.Location, Map, 0x36BD, 20, 10); // Explosion effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                    m.SendMessage("You are hit by an exploding nut!");
                }
            }

            nut.Delete();
        }

        private void NutShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eastern Gray Squirrel surrounds itself with a nut shield! *");
            PlaySound(0x1F6); // Shield sound

            this.VirtualArmor += 20; // Increase virtual armor

            Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
            {
                this.VirtualArmor -= 20; // Remove shield after 10 seconds
            });

            m_NextNutShield = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void SquirrelSwarm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Eastern Gray Squirrel summons a swarm of smaller squirrels! *");
            PlaySound(0x1F6); // Swarm sound

            for (int i = 0; i < 3; i++)
            {
                Squirrel minion = new Squirrel();
                minion.MoveToWorld(Location, Map);
                minion.AI = AIType.AI_Melee;
                minion.FightMode = FightMode.Aggressor;
                minion.Combatant = Combatant;
            }

            m_NextSquirrelSwarm = DateTime.UtcNow + TimeSpan.FromSeconds(60);
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
            m_AbilitiesInitialized = false;
        }
    }

    public class NutItem : Item
    {
        public NutItem() : base(0x1726)
        {
            Movable = false;
        }

        public NutItem(Serial serial) : base(serial)
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
