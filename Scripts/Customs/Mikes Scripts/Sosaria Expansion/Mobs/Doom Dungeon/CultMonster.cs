using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a Cult Monster corpse")]
    public class CultMonster : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextCurseTime;
        private DateTime m_NextSummonTime;
        private DateTime m_NextRitualTime;
        private Point3D m_LastLocation;

        // A deep, sickly violet—marks this horror as otherworldly
        private const int UniqueHue = 1337;

        [Constructable]
        public CultMonster() : base(
            AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4
        )
        {
            Name = "a Cult Monster";
            Title = "of the Broken Star";
            Hue = UniqueHue;

            // ——— Reuse StoneMonster’s randomized bodies & base sounds ———
            switch (Utility.Random(6))
            {
                case 0: Body = 86; BaseSoundID = 634; break;
                case 1: Body = 722; BaseSoundID = 372; break;
                case 2: Body = 59; BaseSoundID = 362; break;
                case 3: Body = 85; BaseSoundID = 639; break;
                case 4: Body = 310; BaseSoundID = 0x482; break;
                default: Body = 83; BaseSoundID = 427; break;
            }

            // ——— Overpowered core stats ———
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(700, 800);

            SetHits(2000, 2400);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 90, 100);

            // ——— Skills ———
            SetSkill(SkillName.EvalInt,   120.0, 140.0);
            SetSkill(SkillName.Magery,    120.0, 140.0);
            SetSkill(SkillName.MagicResist,130.0, 150.0);
            SetSkill(SkillName.Meditation,110.0, 125.0);
            SetSkill(SkillName.Tactics,    95.0, 110.0);
            SetSkill(SkillName.Wrestling,  95.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 100;
            ControlSlots = 5;

            // ——— Initialize cooldowns ———
            var now = DateTime.UtcNow;
            m_NextCurseTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSummonTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextRitualTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));

            // ——— Starter loot ———
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));

            m_LastLocation = this.Location;
        }

        // ——— Aura: Curse on nearby movement ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || m == this || m.Map != Map || !m.InRange(Location, 3))
                return;

            if (m is Mobile target && CanBeHarmful(target, false))
            {
                DoHarmful(target);
                // 25% chance to apply a slowing curse
                if (Utility.RandomDouble() < 0.25)
                {
                    target.SendMessage(0x22, "You feel an ancient curse sap your strength!");
                    target.PlaySound(0x3A);
                    // Slow effect via temporary stamina drain
                    int drain = Utility.RandomMinMax(10, 20);
                    if (target.Stam >= drain) target.Stam -= drain;
                    target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Waist);
                }
            }
        }

        // ——— Main AI loop triggers special abilities ———
        public override void OnThink()
        {
            base.OnThink();

			Mobile target = Combatant as Mobile;
			if (target == null || Map == null || Map == Map.Internal || !Alive)
				return;


            var now = DateTime.UtcNow;

            // Summon 3 acolytes to assist
            if (now >= m_NextSummonTime && InRange(target.Location, 12))
            {
                SummonAcolytes();
                m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Cast a direct lethal curse
            else if (now >= m_NextCurseTime && InRange(target.Location, 10))
            {
                CastLethalCurse(target);
                m_NextCurseTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            // Tear open a ritual rift under the target
            else if (now >= m_NextRitualTime && InRange(target.Location, 8))
            {
                CreateRitualRift(target);
                m_NextRitualTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // ——— Ability: Summon three minor cult phantoms ———
        private void SummonAcolytes()
        {
            Say("*Rise, my children!*");
            PlaySound(0x212);
            for (int i = 0; i < 3; i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z
                );
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var phantom = new Lich(); // Assume you have this class defined
                phantom.MoveToWorld(loc, Map);
                phantom.Combatant = Combatant;
            }
        }

        // ——— Ability: Direct Lethal Curse ———
        private void CastLethalCurse(Mobile target)
        {
            Say("*Feel the doom of the Broken Star!*");
            PlaySound(0x228);
            target.FixedParticles(0x3709, 20, 30, 5032, UniqueHue, 0, EffectLayer.Head);
            DoHarmful(target);

            // 100% chance to apply a potent poison
            if (target is Mobile mTarget)
                mTarget.ApplyPoison(this, Poison.Lethal);
        }

        // ——— Ability: Ritual Rift (ground hazard) ———
        private void CreateRitualRift(Mobile target)
        {
            Say("*Reality shatters!*");
            PlaySound(0x22F);

            var loc = target.Location;
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0
            );

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                // Spawn a circle of toxic gas
                for (int i = 0; i < 5; i++)
                {
                    int dx = Utility.RandomMinMax(-1, 1);
                    int dy = Utility.RandomMinMax(-1, 1);
                    var hazardLoc = new Point3D(loc.X + dx, loc.Y + dy, loc.Z);

                    if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                        hazardLoc.Z = Map.GetAverageZ(hazardLoc.X, hazardLoc.Y);

                    var tile = new PoisonTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(hazardLoc, Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(hazardLoc, Map, EffectItem.DefaultDuration),
                        0x374A, 5, 10, UniqueHue, 0, 5032, 0
                    );
                }
            });
        }

        // ——— Death explosion & lingering hazards ———
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*The cult… persists…*");
                PlaySound(0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 15, 60, UniqueHue, 0, 5052, 0
                );

                // Scatter quicksand and vortex tiles
                for (int i = 0; i < 6; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + dx, Y + dy, Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var tile = (Utility.RandomBool() ? 
                        (Item)new QuicksandTile() : 
                        new VortexTile()
                    );
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // ——— Loot & properties ———
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.03) // 3% chance
                PackItem(new TorsoOfEternalSentinel()); // your custom unique artifact
        }

        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 160.0;
        public override double DispelFocus     => 80.0;

        public CultMonster(Serial serial) : base(serial)
        {
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

            // Reset cooldowns
            var now = DateTime.UtcNow;
            m_NextCurseTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSummonTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextRitualTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            m_LastLocation    = this.Location;
        }
    }
}
