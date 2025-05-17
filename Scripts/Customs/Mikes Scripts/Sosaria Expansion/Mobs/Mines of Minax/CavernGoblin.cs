using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a cavern goblin corpse")]
    public class CavernGoblin : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime m_NextSlamTime;
        private DateTime m_NextPoisonTime;
        private DateTime m_NextGravityWellTime;

        // Unique deep-emerald hue for the Cavern Goblin
        private const int CavernHue = 2400;

        [Constructable]
        public CavernGoblin() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.3, 0.5)
        {
            Name = "a cavern goblin";
            Body = 723;
            BaseSoundID = 0x600;
            Hue = CavernHue;

            // — Vastly Enhanced Stats —
            SetStr(300, 350);
            SetDex(100, 130);
            SetInt(150, 200);

            SetHits(100, 120);
            SetStam(100, 130);
            SetMana(200, 250);

            SetDamage(10, 15);

            // — Damage Types & Resistances —
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 20, 30);

            // — Skills —
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 70;
            ControlSlots = 4;

            // Initialize ability timers
            var now = DateTime.UtcNow;
            m_NextSlamTime         = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextPoisonTime       = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_NextGravityWellTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));

            // Loot: rich ore, gems, rare goblin artifacts
            PackItem(new IronOre(Utility.RandomMinMax(20, 30)));
            PackItem(new Granite(Utility.RandomMinMax(10, 20)));
            AddLoot(LootPack.Gems);
            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new GoblinShamansMantle());
        }

        // — Seismic Slam: AoE physical burst + knockback —
        public void SeismicSlam()
        {
            PlaySound(0x2D6); // seismic roar
            FixedParticles(0x3728, 10, 30, 5052, CavernHue, 0, EffectLayer.CenterFeet);

            var list = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 5))
            {
                if (m != this && CanBeHarmful(m, false))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0);

                    // Knockback
                    var dx = target.X - X;
                    var dy = target.Y - Y;
                    var nx = target.X + (dx != 0 ? dx / Math.Abs(dx) : 0);
                    var ny = target.Y + (dy != 0 ? dy / Math.Abs(dy) : 0);
                    target.MoveToWorld(new Point3D(nx, ny, target.Z), Map);
                }
            }
        }

        // — Poison Spit: Targeted poison plus lingering toxic cloud (ToxicGasTile) —
        public void PoisonSpit()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*Sssspit!*");
            PlaySound(0x658);
            target.FixedParticles(0x373A, 1, 15, 0x22, CavernHue, 0, EffectLayer.Waist);

            DoHarmful(target);
            target.ApplyPoison(target, Poison.Lethal);

            // Spawn a toxic gas hazard at victim's feet
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                var loc = target.Location;
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var gas = new ToxicGasTile();
                    gas.Hue = CavernHue;
                    gas.MoveToWorld(loc, Map);
                }
            });
        }

        // — Gravity Well: Creates a VortexTile that pulls in nearby foes —
        public void GravityWell()
        {
            if (!(Combatant is Mobile target)) return;

            Say("*The cavern draws you in…*");
            PlaySound(0x208);

            var loc = target.Location;
            Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
            {
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var vortex = new VortexTile();
                    vortex.Hue = CavernHue;
                    vortex.MoveToWorld(loc, Map);
                }
            });
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal) 
                return;

            var now = DateTime.UtcNow;

            // Priority: Slam if in melee range
            if (now >= m_NextSlamTime && InRange(Combatant.Location, 6))
            {
                SeismicSlam();
                m_NextSlamTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            }
            else if (now >= m_NextPoisonTime && InRange(Combatant.Location, 12))
            {
                PoisonSpit();
                m_NextPoisonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 18));
            }
            else if (now >= m_NextGravityWellTime)
            {
                GravityWell();
                m_NextGravityWellTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 26));
            }
        }

        // — Darkness Aura: slows any foe who moves within 2 tiles —
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || m.Map != Map || !m.InRange(Location, 2) || !CanBeHarmful(m, false)) 
                return;

            if (m is Mobile target)
            {
                // Slow effect
                target.SendMessage(0x22, "The cavern’s shadow drags at your feet!");
                target.Paralyze(TimeSpan.FromSeconds(1.0)); 
            }
        }

        // — Death: cave‑in of Quicksand hazards around the corpse —
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            PlaySound(0x2A3);
            for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
            {
                var dx = Utility.RandomMinMax(-2, 2);
                var dy = Utility.RandomMinMax(-2, 2);
                var loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var qs = new QuicksandTile();
                qs.Hue = CavernHue;
                qs.MoveToWorld(loc, Map);
            }
        }

        public override int GetAngerSound()   { return 0x600; }
        public override int GetIdleSound()    { return 0x600; }
        public override int GetAttackSound()  { return 0x5FD; }
        public override int GetHurtSound()    { return 0x5FF; }
        public override int GetDeathSound()   { return 0x5FE; }

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 5;
        public override int Meat => 2;
        public override TribeType Tribe => TribeType.GrayGoblin;
		
		public CavernGoblin(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
            // Re‑init timers
            var now = DateTime.UtcNow;
            m_NextSlamTime        = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextPoisonTime      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_NextGravityWellTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
        }
    }
}
