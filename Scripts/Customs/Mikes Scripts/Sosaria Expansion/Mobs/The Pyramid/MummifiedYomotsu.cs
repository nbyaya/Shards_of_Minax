using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a mummified yomotsu corpse")]
    public class MummifiedYomotsu : BaseCreature
    {
        private DateTime m_NextCurseTime;
        private DateTime m_NextSandstormTime;
        private DateTime m_NextSummonTime;

        // Dusty‑gold mummy hue
        private const int UniqueHue = 2101;

        [Constructable]
        public MummifiedYomotsu() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a mummified yomotsu";
            Body = 255;
            BaseSoundID = 0x452;
            Hue = UniqueHue;

            // Stats
            SetStr(700, 850);
            SetDex(200, 300);
            SetInt(50, 100);

            SetHits(1200, 1500);
            SetStam(200, 250);
            SetMana(100, 200);

            SetDamage(25, 35);

            // Damage types
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 70, 90);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 40, 60);

            // Skills
            SetSkill(SkillName.Anatomy, 120.1, 130.0);
            SetSkill(SkillName.Tactics, 120.1, 130.0);
            SetSkill(SkillName.Wrestling, 120.1, 130.0);
            SetSkill(SkillName.Poisoning, 100.1, 110.0);
            SetSkill(SkillName.MagicResist, 110.1, 125.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 90;
            ControlSlots = 6;

            // Cooldowns
            m_NextCurseTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSummonTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 60));

            // Loot
            PackItem(new Bone(Utility.RandomMinMax(5, 10)));
            PackItem(new GirdleOfTheMountainEcho());   // assumes you’ve defined this
            PackItem(new TheShadowsLastStep());   // assumes you’ve defined this

            if (Utility.RandomDouble() < 0.02)
                PackItem(new Gloambite()); // rare artifact
        }

        // Bind with bandages & leech on melee
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // 15% Bandage Bind
            if (Utility.RandomDouble() < 0.15 && defender is Mobile target)
            {
                DoHarmful(target);
                target.FixedEffect(0x37B9, 10, 5);
                target.SendMessage(0x22, "Ancient bandages wrap around you, binding your limbs!");
                target.Paralyze(TimeSpan.FromSeconds(3.0));
            }

            // 10% Soul Drain
            if (Utility.RandomDouble() < 0.10 && defender is Mobile leechTarget)
            {
                int drain = Utility.RandomMinMax(10, 20);
                if (leechTarget.Mana < drain) drain = leechTarget.Mana;
                if (drain > 0)
                {
                    leechTarget.Mana -= drain;
                    leechTarget.SendMessage(0x22, "You feel your life force drain away!");
                    this.Hits += drain;
                    this.PlaySound(0x1F9);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Map == Map.Internal || Combatant == null)
                return;

            // Pharaoh's Curse AoE
            if (DateTime.UtcNow >= m_NextCurseTime)
            {
                PharaohsCurse();
                m_NextCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }

            // Sandstorm hazards if target is near
            if (DateTime.UtcNow >= m_NextSandstormTime 
                && Combatant is Mobile combatTarget 
                && InRange(combatTarget.Location, 12))
            {
                SandstormAttack();
                m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }

            // Scarab Swarm when below 50% health
            if (Hits < HitsMax / 2 && DateTime.UtcNow >= m_NextSummonTime)
            {
                SummonScarabSwarm();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 75));
            }
        }

        // AoE poison burst + Lethal poison
        private void PharaohsCurse()
        {
            Say("The curse of the pharaoh descends!");
            PlaySound(0x22F);

            var targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) 
                    && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            foreach (Mobile t in targets)
            {
                DoHarmful(t);
                AOS.Damage(t, this, Utility.RandomMinMax(20, 35), 0, 0, 0, 0, 100);
                t.ApplyPoison(this, Poison.Lethal);
                t.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                t.PlaySound(0x23F);
            }
        }

        // Spawn quicksand tiles in a 7×7 diamond shape
        private void SandstormAttack()
        {
            Say("The sands of time shall swallow you!");
            PlaySound(0x307);

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 10, 60, UniqueHue, 0, 5039, 0);

            for (int x = -3; x <= 3; x++)
            {
                for (int y = -3; y <= 3; y++)
                {
                    if (Math.Abs(x) + Math.Abs(y) <= 4)
                    {
                        Point3D loc = new Point3D(X + x, Y + y, Z);
                        if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                            loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                        QuicksandTile tile = new QuicksandTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(loc, Map);
                    }
                }
            }
        }

        // Summon 3–6 scarabs around itself
        private void SummonScarabSwarm()
        {
            Say("Rise, my scarabs!");
            PlaySound(0x56F);

            for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
            {
                Skeleton crawly = new Skeleton();  // assumes a Scarab class exists
                crawly.Hue = UniqueHue;

                int dx = Utility.RandomMinMax(-2, 2);
                int dy = Utility.RandomMinMax(-2, 2);
                Point3D spawn = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                crawly.MoveToWorld(spawn, Map);
            }
        }

        // On‑death explosion of poison & necro‑flamestrike tiles
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("My curse endures...");
                PlaySound(0x211);

                for (int i = 0; i < Utility.RandomMinMax(5, 8); i++)
                {
                    int dx = Utility.RandomMinMax(-4, 4);
                    int dy = Utility.RandomMinMax(-4, 4);
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    // Poison hazard
                    PoisonTile poison = new PoisonTile();
                    poison.Hue = UniqueHue;
                    poison.MoveToWorld(loc, Map);

                    // Necromantic flamestrike hazard
                    NecromanticFlamestrikeTile flame = new NecromanticFlamestrikeTile();
                    flame.Hue = UniqueHue;
                    flame.MoveToWorld(loc, Map);
                }
            }
            base.OnDeath(c);
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;

        public MummifiedYomotsu(Serial serial) : base(serial) { }

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
