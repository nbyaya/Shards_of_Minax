using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
    public class NecromanticFlamestrikeTile : Item
    {
        private Timer m_AnimationTimer;
        private Timer m_DeleteTimer;
        public int MaxSkeletons { get; set; }
        public TimeSpan AutoDelete { get; set; }

        [Constructable]
        public NecromanticFlamestrikeTile() : base(0x1A8) // Use an appropriate tile ID
        {
            Movable = false;
            Name = "a necromantic flamestrike";
            Hue = 33; // Dark red hue

            MaxSkeletons = 5; // Maximum number of skeletons to summon
            AutoDelete = TimeSpan.FromSeconds(30);

            m_AnimationTimer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), PerformFlamestrike);
            StartDeleteTimer();
        }

        private void PerformFlamestrike()
        {
            if (Deleted) return;

            // Perform the flamestrike animation
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 20, 33, 0); // Flamestrike effect with dark red hue
            Effects.PlaySound(Location, Map, 0x208); // Flamestrike sound

            // Summon a skeleton if we haven't reached the maximum
            if (CountNearbySkeletons() < MaxSkeletons)
            {
                Skeleton skeleton = new Skeleton();
                skeleton.MoveToWorld(Location, Map);
                skeleton.Combatant = null; // Ensure the skeleton doesn't immediately attack
                Effects.SendLocationParticles(EffectItem.Create(skeleton.Location, skeleton.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
            }
        }

        private int CountNearbySkeletons()
        {
            int count = 0;
            IPooledEnumerable eable = GetMobilesInRange(10); // Count skeletons within 10 tiles
            foreach (Mobile m in eable)
            {
                if (m is Skeleton)
                    count++;
            }
            eable.Free();
            return count;
        }

        private void StartDeleteTimer()
        {
            if (m_DeleteTimer != null)
                m_DeleteTimer.Stop();

            m_DeleteTimer = Timer.DelayCall(AutoDelete, DeleteTimer);
        }

        private void DeleteTimer()
        {
            this.Delete();
        }

        public override void OnDelete()
        {
            if (m_AnimationTimer != null)
                m_AnimationTimer.Stop();

            if (m_DeleteTimer != null)
                m_DeleteTimer.Stop();

            base.OnDelete();
        }

        public NecromanticFlamestrikeTile(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(MaxSkeletons);
            writer.Write(AutoDelete);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            MaxSkeletons = reader.ReadInt();
            AutoDelete = reader.ReadTimeSpan();
            m_AnimationTimer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), PerformFlamestrike);
            StartDeleteTimer();
        }
    }
}