using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a void webspinner corpse")]
    public class VoidWebspinner : BaseCreature
    {
        private DateTime m_NextSnareTime;
        private DateTime m_NextStormTime;
        private DateTime m_NextVolleyTime;

        private const int UniqueHue = 1175; // Deep void-purple

        [Constructable]
        public VoidWebspinner()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a void webspinner";
            Body = 28;                // Giant spider body
            BaseSoundID = 0x388;      // Spider sounds
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(1500, 1800);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 30);
            SetDamageType(ResistanceType.Poison, 30);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Skills
            SetSkill(SkillName.Poisoning, 110.1, 120.0);
            SetSkill(SkillName.MagicResist, 110.1, 120.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 70;
            ControlSlots = 5;

            // Ability cooldowns
            m_NextSnareTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextStormTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            // Loot
            PackItem(new SpidersSilk(Utility.RandomMinMax(20, 30)));
            PackItem(new VoidCore()); // Unique void-themed crafting component
        }

        public override bool BleedImmune   { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override Poison HitPoison    { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 6; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target && this.Map != null && Alive)
            {
                DateTime now = DateTime.UtcNow;

                if (now >= m_NextVolleyTime && InRange(target.Location, 12))
                {
                    WebVolley();
                    m_NextVolleyTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
                }
                else if (now >= m_NextStormTime && InRange(target.Location, 10))
                {
                    VoidStorm();
                    m_NextStormTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }
                else if (now >= m_NextSnareTime && InRange(target.Location, 8))
                {
                    VoidSnare();
                    m_NextSnareTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                }
            }
        }

        // --- Ability: Void Snare ---
        private void VoidSnare()
        {
            this.Say("*the air shimmers with strands of void silk*");
            PlaySound(0x212); // Web-like snap

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);

                    // Place a trapping web at their feet
                    TrapWeb web = new TrapWeb();
                    web.Hue = UniqueHue;
                    web.MoveToWorld(m.Location, this.Map);

                    if (m is Mobile mob) mob.SendMessage("You are ensnared by void webs!");
                }
            }
            eable.Free();
        }

        // --- Ability: Void Storm ---
        private void VoidStorm()
        {
            if (!(Combatant is Mobile target)) return;

            this.Say("*void energies surge outward!*");
            PlaySound(0x213); // Crackling void burst
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5032, 0);

            // Delay before spawning hazardous vortices
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                int count = Utility.RandomMinMax(5, 8);
                for (int i = 0; i < count; i++)
                {
                    int xOff = Utility.RandomMinMax(-3, 3);
                    int yOff = Utility.RandomMinMax(-3, 3);
                    Point3D loc = new Point3D(
                        target.X + xOff,
                        target.Y + yOff,
                        target.Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    VortexTile vortex = new VortexTile();
                    vortex.Hue = UniqueHue;
                    vortex.MoveToWorld(loc, this.Map);
                }
            });
        }

        // --- Ability: Web Volley ---
        private void WebVolley()
        {
            this.Say("*the void webspinner hurls razor-sharp void webs!*");
            PlaySound(0x214); // Volley sound

            var targets = new List<Mobile>();
            if (Combatant is Mobile primary && CanBeHarmful(primary, false) && SpellHelper.ValidIndirectTarget(this, primary))
                targets.Add(primary);

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 12);
            foreach (Mobile m in eable)
            {
                if (m != this && targets.Count < 5 && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                m.SendMessage("You are struck by shards of void silk!");
                m.ApplyPoison(this, Poison.Lethal);

                // Visual lash effect
                m.FixedParticles(0x3779, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
            }
        }

        // --- Death Effect ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*threads of void unspool into nothingness!*");
                PlaySound(0x215); // Final unravel sound

                for (int i = 0; i < 5; i++)
                {
                    int xOff = Utility.RandomMinMax(-3, 3);
                    int yOff = Utility.RandomMinMax(-3, 3);
                    Point3D loc = new Point3D(X + xOff, Y + yOff, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    // Spawn both web traps and void vortex hazards
                    TrapWeb web = new TrapWeb();
                    web.Hue = UniqueHue;
                    web.MoveToWorld(loc, Map);

                    VortexTile vortex = new VortexTile();
                    vortex.Hue = UniqueHue;
                    vortex.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        public VoidWebspinner(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset ability timers after reload
            m_NextSnareTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextStormTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextVolleyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
