using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a smoldering kitsune corpse")]
    public class EmberKitsune : BaseCreature
    {
        private DateTime m_NextTailStrike;
        private DateTime m_NextHowl;
        private DateTime m_LastIllusion;
        private List<Mobile> m_Illusions = new List<Mobile>();
        private const int UniqueHue = 1359; // Crimson with black accents

        [Constructable]
        public EmberKitsune() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Ember Kitsune";
            Body = 246; // Base kitsune body
            Hue = UniqueHue;

            SetStr(400, 550);
            SetDex(300, 350);
            SetInt(500, 650);

            SetHits(2500, 3000);
            SetStam(300, 350);
            SetMana(500, 650);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 100, 110); // Immune to fire
            SetResistance(ResistanceType.Cold, -20, 0);    // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 120.1, 130.0);
            SetSkill(SkillName.Magery, 120.1, 130.0);
            SetSkill(SkillName.MagicResist, 125.0, 135.0);
            SetSkill(SkillName.Tactics, 115.0, 125.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 80;
            ControlSlots = 5;

            m_NextTailStrike = DateTime.UtcNow;
            m_NextHowl = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));

            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            AddItem(new LightSource());
        }

        public EmberKitsune(Serial serial)
            : base(serial)
        {
        }
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextTailStrike)
            {
                NineTailStrike();
                m_NextTailStrike = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }

            if (DateTime.UtcNow - m_LastIllusion > TimeSpan.FromSeconds(30))
            {
                CreateCursedIllusions();
                m_LastIllusion = DateTime.UtcNow;
            }
        }

        private void NineTailStrike()
        {
            if (Combatant is Mobile target && InRange(target, 2))
            {
                this.MovingParticles(target, 0x36D4, 7, 0, false, true, UniqueHue, 0, 9502, 4019, 0x160, 0);
                this.PlaySound(0x44B);

                int damage = Utility.RandomMinMax(40, 60);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0); // Pure fire damage

                if (target.Alive && Utility.RandomDouble() < 0.4)
                {
                    target.SendMessage("The searing tails scorch your flesh!");
                    target.AddStatMod(new StatMod(StatType.Str, "TailBurn", -20, TimeSpan.FromSeconds(10)));
                }
            }
        }

        private void CreateCursedIllusions()
        {
            ClearIllusions();

            for (int i = 0; i < 3; i++)
            {
                EmberIllusion illusion = new EmberIllusion(this);
                illusion.MoveToWorld(GetSpawnPosition(2), this.Map);
                m_Illusions.Add(illusion);
            }

            this.PlaySound(0x1FB);
            this.FixedParticles(0x376A, 10, 15, 5052, EffectLayer.Waist);
        }

        private void ClearIllusions()
        {
            foreach (Mobile illusion in m_Illusions)
            {
                if (illusion != null && !illusion.Deleted)
                    illusion.Delete();
            }
            m_Illusions.Clear();
        }

        public override void OnDeath(Container c)
        {
            ClearIllusions();
            CreateChaoticTeleportTile();

            if (Utility.RandomDouble() < 0.1)
                c.DropItem(new MaxxiaScroll());

            base.OnDeath(c);
        }

        private void CreateChaoticTeleportTile()
        {
            List<Point3D> vortexPoints = new List<Point3D>();
            for (int i = 0; i < 8; i++)
            {
                Point3D p = new Point3D(
                    this.X + Utility.RandomMinMax(-3, 3),
                    this.Y + Utility.RandomMinMax(-3, 3),
                    this.Z
                );

                if (Map.CanFit(p, 16))
                {
                    new ChaoticTeleportTile().MoveToWorld(p, this.Map);
                    vortexPoints.Add(p);
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(1), () => {
                foreach (Point3D p in vortexPoints)
                {
                    Effects.SendLocationParticles(EffectItem.Create(p, this.Map, EffectItem.DefaultDuration), 
                        0x3709, 10, 30, UniqueHue, 0, 5052, 0);
                }
            });
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster is PlayerMobile && Utility.RandomDouble() < 0.3)
            {
                caster.SendMessage("The kitsune's flames reflect your spell back at you!");
                AOS.Damage(caster, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 2);

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }	
        // Remaining standard methods (serialization, sound IDs, etc)...
    }

    public class EmberIllusion : BaseCreature
    {
        public EmberIllusion(Mobile master) : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a cursed illusion";
            Body = master.Body;
            Hue = 1359;
            SetStr(200);
            SetDex(200);
            SetInt(100);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Fire, 100);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Energy, 100);

            Timer.DelayCall(TimeSpan.FromSeconds(30), Delete);
        }
        public EmberIllusion(Serial serial)
            : base(serial)
        {
        }
        public override void OnDeath(Container c)
        {
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, 1359, 0, 5052, 0);
            base.OnDeath(c);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }	        
    }
}