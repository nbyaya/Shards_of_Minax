using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mummy render corpse")]
    public class MummyRender : BaseCreature
    {
        private DateTime m_NextMiasmaTime;
        private DateTime m_NextEntombTime;
        private DateTime m_NextScarabTime;
        private Point3D m_LastLocation;

        // Sand‑green hue
        private const int UniqueHue = 1963;

        [Constructable]
        public MummyRender()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            Name = "a mummy render";
            Body = 315;
            Hue = UniqueHue;

            // Stats
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(6000);
            SetStam(300, 350);
            SetMana(200, 300);

            // Physical + Poison
            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            // High defenses
            SetResistance(ResistanceType.Physical, 85, 95);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Combat skills
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.Tactics,   100.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Poisoning, 100.0, 110.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Ability cooldowns
            m_NextMiasmaTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextEntombTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextScarabTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation    = this.Location;

            // Loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
            PackGold(800, 1200);
        }

        // ** Poisonous Miasma ** — AoE poison around MummyRender
        public override void OnThink()
        {
            base.OnThink();

            if (Map == null || !Alive || Combatant == null) return;

            // ** Poisonous Miasma **
            if (DateTime.UtcNow >= m_NextMiasmaTime)
            {
                m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
                PoisonousMiasma();
            }

            // ** Entombing Grasp **
            if (DateTime.UtcNow >= m_NextEntombTime && this.InRange(Combatant.Location, 3))
            {
                m_NextEntombTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
                EntombingGrasp();
            }

            // ** Scarab Swarm **
            if (DateTime.UtcNow >= m_NextScarabTime)
            {
                m_NextScarabTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                ScarabSwarm();
            }

            // ** Quicksand Trail **
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                LeaveQuicksand();
            }
            m_LastLocation = this.Location;
        }

        private void PoisonousMiasma()
        {
            this.Say("*Hssssss…*");
            this.PlaySound(0x212);

            var list = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(this.Location, 5))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && m is Mobile target)
                    list.Add(target);
            }

            foreach (var target in list)
            {
                DoHarmful(target);
                target.ApplyPoison(this, Poison.Deadly);
                target.SendMessage(0x22, "You gasp as deadly miasma sears your lungs!");
                target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);
            }
        }

        private void EntombingGrasp()
        {
            if (!(Combatant is Mobile target)) return;

            this.Say("*Taste eternal bondage!*");
            this.PlaySound(0x2D4);
            DoHarmful(target);

            target.Paralyze(TimeSpan.FromSeconds(4.0));
            target.SendMessage(0x22, "Your limbs stiffen as ancient linen wraps you tight!");
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration),
                0x376A, 8, 30, UniqueHue, 0, 5039, 0
            );
        }

        private void ScarabSwarm()
        {
            if (!(Combatant is Mobile target)) return;

            this.Say("*Rise, my minions!*");
            this.PlaySound(0x64A);

            // Drop PoisonTiles in a small circle around the target
            for (int i = 0; i < 5; i++)
            {
                int xOff = Utility.RandomMinMax(-2, 2);
                int yOff = Utility.RandomMinMax(-2, 2);
                var loc = new Point3D(target.X + xOff, target.Y + yOff, target.Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, this.Map);
            }
        }

        private void LeaveQuicksand()
        {
            var loc = m_LastLocation;
            if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                loc.Z = Map.GetAverageZ(loc.X, loc.Y);

            var qs = new QuicksandTile();
            qs.Hue = UniqueHue;
            qs.MoveToWorld(loc, this.Map);
        }

        // ** Life‑Drain on melee hit **
		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			if (defender != null && SpellHelper.ValidIndirectTarget(this, defender))
			{
				Mobile target = defender; // Explicit cast
				int drain = Utility.RandomMinMax(15, 25);
				AOS.Damage(target, this, drain, 0, 0, 0, 0, 100);
				this.Heal(drain / 2);
				target.SendMessage(0x22, "The mummy’s curse saps your vitality!");
				target.PlaySound(0x4EC);
			}
		}


        // ** Death explosion & hazards **
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;
            this.Say("*My curse… endures!*");
            this.PlaySound(0x354);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5052, 0
            );

            // Scatter necromantic hazard tiles
            for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(this.X + xOff, this.Y + yOff, this.Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new NecromanticFlamestrikeTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, this.Map);
            }
        }

        // ** Loot & properties **
        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool BardImmune     => true;
        public override bool Unprovokable   => true;
        public override bool AreaPeaceImmune=> true;

        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new MoonmarkAegis()); // example unique artifact
        }

        // ** Sounds **
        public override int GetAttackSound() => 0x34C;
        public override int GetHurtSound()   => 0x354;
        public override int GetAngerSound()  => 0x34C;
        public override int GetIdleSound()   => 0x34C;
        public override int GetDeathSound()  => 0x354;

        public MummyRender(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            m_NextMiasmaTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextEntombTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextScarabTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation    = this.Location;
        }
    }
}
